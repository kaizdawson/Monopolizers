using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Monopolizers.Service.Contract;
using Monopolizers.Repository.Repositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Threading.Tasks;
using Monopolizers.Common.DTO;

namespace Monopolizers.API.Controllers
{
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

        [HttpPost("webhook/payos")]
        public async Task<IActionResult> HandleWebhook()
        {
            // 1. Đọc raw JSON từ body
            string rawBody;
            using (var reader = new StreamReader(Request.Body))
            {
                rawBody = await reader.ReadToEndAsync();
            }

            // 2. Parse JSON → Remove "signature"
            var jsonObject = JObject.Parse(rawBody);
            var receivedSignature = jsonObject["signature"]?.ToString();
            jsonObject.Remove("signature"); // Xoá "signature" trước khi ký lại
            var jsonWithoutSignature = jsonObject.ToString(Formatting.None);

            // 3. So sánh chữ ký
            var expectedSignature = _payosService.GenerateWebhookSignature(jsonWithoutSignature);
            if (!string.Equals(receivedSignature, expectedSignature, StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning("❌ Signature không hợp lệ");
                _logger.LogWarning("Received: " + receivedSignature);
                _logger.LogWarning("Expected: " + expectedSignature);
                _logger.LogWarning("Raw JSON: " + rawBody);
                return BadRequest("Invalid signature");
            }

            // 4. Sau khi xác thực OK, parse JSON để xử lý dữ liệu
            var parsedObject = JsonConvert.DeserializeObject<PayOSWebhookInput>(rawBody);
            var dto = parsedObject?.Data;

            if (dto == null || !string.Equals(dto.Status, "PAID", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogInformation("⚠️ Trạng thái không phải PAID => Bỏ qua");
                return Ok("Ignored");
            }

            try
            {
                var username = dto.Description?.Replace("Nap vi", "", StringComparison.OrdinalIgnoreCase).Trim();
                var user = await _accountRepository.FindByUsernameAsync(username);
                if (user == null)
                    return BadRequest("User not found");

                await _walletService.AddBalanceFromPayOSAsync(user.Id, dto.Amount);
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
