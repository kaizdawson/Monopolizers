using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using Monopolizers.Service.Contract;
using Monopolizers.Common.DTO;
using Monopolizers.Repository.Repositories;

namespace Monopolizers.API.Controllers
{
    [Route("api/webhook/payos")]
    [ApiController]
    public class PayOSWebhookController : ControllerBase
    {
        private readonly IWalletService _walletService;
        private readonly ILogger<PayOSWebhookController> _logger;
        private readonly IAccountRepository _accountRepository;
        private const string PAYOS_CLIENT_SECRET = "b37a42af-5e0a-43c4-8a1a-604c918ece84"; 

        public PayOSWebhookController(IWalletService walletService,IAccountRepository accountRepository, ILogger<PayOSWebhookController> logger)
        {
            _walletService = walletService;
            _accountRepository = accountRepository; 
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> HandleWebhook([FromBody] PayOSWebhookDTO dto)
        {
            var signatureHeader = Request.Headers["x-signature"].FirstOrDefault();
            if (string.IsNullOrEmpty(signatureHeader))
                return BadRequest("Missing signature");

            var rawData = $"{dto.orderCode}{dto.amount}{dto.description}{dto.status}{dto.transactionId}{dto.time}";
            var secretKey = PAYOS_CLIENT_SECRET;

            var calculatedSignature = CalculateHMACSHA256(rawData, secretKey);

            if (signatureHeader != calculatedSignature)
            {
                _logger.LogWarning("Signature mismatch");
                return BadRequest("Invalid signature");
            }

            if (dto.status == "PAID")
            {
                var username = dto.description?.Replace("Nap vi: ", "").Trim(); 

                if (string.IsNullOrEmpty(username))
                    return BadRequest("Missing username in description");

                var user = await _accountRepository.FindByUsernameAsync(username);
                if (user == null)
                    return BadRequest("User not found");

                var userId = user.Id;

                var amountDecimal = dto.amount / 100m;

                await _walletService.AddBalanceFromPayOSAsync(userId, amountDecimal);

                _logger.LogInformation($"Đã cộng tiền: {amountDecimal} vào ví user {userId}");
                return Ok();
            }


            return Ok("Ignored non-paid status");
        }

        private static string CalculateHMACSHA256(string data, string secretKey)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
            {
                var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}
