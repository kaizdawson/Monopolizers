using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Monopolizers.Service.Contract;
using Monopolizers.Repository.Repositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Threading.Tasks;
using Monopolizers.Common.DTO;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cors;

namespace Monopolizers.API.Controllers
{
    [EnableCors("AllowWebhook")]
    [Route("api/webhook/payos")]
    [ApiController]
    public class PayOSWebhookController : ControllerBase
    {
        private readonly IWalletService _walletService;
        private readonly ILogger<PayOSWebhookController> _logger;
        private readonly IAccountRepository _accountRepository;
        private readonly PayOSService _payosService;

        public PayOSWebhookController(
            IWalletService walletService,
            IAccountRepository accountRepository,
            ILogger<PayOSWebhookController> logger,
            PayOSService payosService)
        {
            _walletService = walletService;
            _accountRepository = accountRepository;
            _logger = logger;
            _payosService = payosService;
        }

        [HttpPost]
        public async Task<IActionResult> HandleWebhook()
        {
            _logger.LogInformation("📩 Webhook received at: " + DateTime.UtcNow);
            string rawBody;
            using (var reader = new StreamReader(Request.Body))
            {
                rawBody = await reader.ReadToEndAsync();
            }

            // Parse JSON để lấy "signature" từ bản gốc
            var jObject = JObject.Parse(rawBody);
            var receivedSignature = jObject["signature"]?.ToString();

            if (string.IsNullOrEmpty(receivedSignature))
            {
                _logger.LogWarning("❌ Không tìm thấy signature trong payload");
                return BadRequest("Missing signature");
            }

            // Tạo bản sao rawBody KHÔNG có "signature" để xác thực
            var dataJObject = jObject["data"] as JObject;
            if (dataJObject == null)
            {
                _logger.LogWarning("❌ Không tìm thấy object `data` trong payload");
                return BadRequest("Missing data");
            }

            var unsignedData = dataJObject.ToObject<Dictionary<string, object>>();
            var expectedSignature = _payosService.GenerateWebhookSignature(unsignedData);


            if (!string.Equals(receivedSignature, expectedSignature, StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning("❌ Signature không hợp lệ");
                _logger.LogWarning("Received: " + receivedSignature);
                _logger.LogWarning("Expected: " + expectedSignature);
                _logger.LogWarning("Raw JSON: " + rawBody);
                return BadRequest("Invalid signature");
            }


            var parsed = JsonConvert.DeserializeObject<PayOSWebhookInput>(rawBody);
            var dto = parsed?.Data;

            if (dto == null || !parsed.Success)
            {
                _logger.LogInformation($"⚠️ Giao dịch không thành công hoặc thiếu dữ liệu. Status = {dto?.Status}");
                return Ok("Ignored");
            }


            try
            {
                _logger.LogInformation("📥 Bắt đầu xử lý dữ liệu webhook");

                var description = dto.Description ?? "";
                var username = description.Split("Nap vi", StringSplitOptions.RemoveEmptyEntries)
                                          .LastOrDefault()?.Trim();

                _logger.LogInformation($"🔍 Đã parse ra username: {username}");

                if (string.IsNullOrEmpty(username))
                {
                    _logger.LogWarning("❌ Không tìm được username trong description: " + description);
                    return BadRequest("Username parsing failed");
                }

                var user = await _accountRepository.FindByUsernameAsync(username);
                if (user == null)
                {
                    _logger.LogWarning("❌ Không tìm thấy user với username: " + username);
                    return BadRequest("User not found");
                }

                _logger.LogInformation($"🔄 Tìm thấy userId = {user.Id}, tiến hành cộng tiền");

                await _walletService.AddBalanceFromPayOSAsync(user.Id, dto.Amount, dto.OrderCode.ToString());
                _logger.LogInformation($"✅ Cộng {dto.Amount} vào ví userId = {user.Id}");
                return Ok("Nạp ví thành công");
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Lỗi xử lý webhook");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
