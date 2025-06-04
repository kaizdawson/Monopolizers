using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Monopolizers.Common.DTO;
using Monopolizers.Repository.DB;
using Monopolizers.Repository.Models;
using Monopolizers.Repository.Repositories;
using Monopolizers.Service.Contract;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Monopolizers.API.UserControllers
{
    [Route("api/customer/wallet")]
    [ApiController]
    [Authorize]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;
        private readonly IVnPayService _vnPayService;
        public WalletController(IWalletService walletService, IVnPayService vnPayService)
        {
            _walletService = walletService;
            _vnPayService = vnPayService;
        }

        [HttpGet]
        public async Task<ActionResult<WalletDTO>> GetWalletBalance()
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Bạn chưa đăng nhập hoặc token không hợp lệ.");
            }

            var walletBalance = await _walletService.GetWalletBalanceAsync(userId);
            if (walletBalance == null)
            {
                return NotFound("Không có Ví.");
            }

            return Ok(walletBalance);
        }

        [Authorize]
        [HttpPost("CreateVNPayPayment")]
        public IActionResult CreateVNPayPayment([FromBody] CreateVNPayRequestDTO request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { Message = "Bạn chưa đăng nhập hoặc token không hợp lệ." });
            }
            if (request.Amount <= 0)
            {
                return BadRequest(new { Message = "Số tiền không hợp lệ." });
            }

            var paymentRequest = new VnPaymentRequestModel
            {
                Amount = request.Amount,
                CreatedTime = DateTime.UtcNow
            };
            string paymentUrl = _vnPayService.CreatePaymentUrl(HttpContext, paymentRequest, userId);

            return Ok(new { PaymentUrl = paymentUrl });
        }

        [AllowAnonymous]
        [HttpGet("VNPayReturn")]
        public async Task<IActionResult> VNPayReturn()
        {
            try
            {
                var response = _vnPayService.PaymentExecute(Request.Query);
                if (!response.Success)
                {
                    return BadRequest(new { Message = "Giao dịch thất bại hoặc bị từ chối." });
                }

                // Lấy UserId từ VNPay trả về
                string vnp_UserId = response.OrderDescription?.Replace("Thanh toán VNPay - UserId: ", "").Trim();
                if (string.IsNullOrEmpty(vnp_UserId))
                    return BadRequest(new { Message = "VNPay không gửi UserId hợp lệ." });

                if (!decimal.TryParse(Request.Query["vnp_Amount"], out var vnpAmountRaw))
                    return BadRequest(new { Message = "VNPay không gửi số tiền hợp lệ." });

                decimal vnpAmount = vnpAmountRaw / 100;

                var result = await _walletService.DepositAsync(vnp_UserId, vnpAmount);

                if (result != "Success")
                    return BadRequest(new { Message = result });

                var redirectUrl = $"http://localhost:5173/deposite" +
                                  $"?Status=Success" +
                                  $"&vnp_ResponseCode=00" +
                                  $"&vnp_Amount={vnpAmountRaw}" +
                                  $"&vnp_BankCode={Request.Query["vnp_BankCode"]}" +
                                  $"&vnp_BankTranNo={Request.Query["vnp_BankTranNo"]}" +
                                  $"&vnp_TransactionNo={Request.Query["vnp_TransactionNo"]}" +
                                  $"&vnp_PayDate={Request.Query["vnp_PayDate"]}";

                return Redirect(redirectUrl);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Lỗi xử lý VNPay Return", Error = ex.Message });
            }
        }


    }
}
