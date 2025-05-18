using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Monopolizers.Models;
using Monopolizers.Repositories;

namespace Monopolizers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository AccountRepository;

        public AccountController(IAccountRepository repo)
        {
            AccountRepository = repo;
        }
        [AllowAnonymous]
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(SignUpModel signUpModel)
        {
            if (!ModelState.IsValid)
            {
                var firstError = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .FirstOrDefault();

                return BadRequest(new { message = firstError });
            }

            var result = await AccountRepository.SignUpAsync(signUpModel);
            if (result.Succeeded)
            {
                return Ok(new { message = "Đăng ký thành công!" });
            }

            var firstIdentityError = result.Errors.Select(e => e.Description).FirstOrDefault();
            return BadRequest(new { message = firstIdentityError });
        }

        [AllowAnonymous]
        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(SignInModel signInModel)
        {
            var result = await AccountRepository.SignInAsync(signInModel);

            if (result == "Không tìm thấy tài khoản này!" || result == "Sai Mật Khẩu")
            {
                return Unauthorized(new { message = result });
            }

            // Trường hợp thành công, trả về token
            return Ok(new { token = result });
        }

    }
}
