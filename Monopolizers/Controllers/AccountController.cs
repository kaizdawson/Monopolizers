using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Monopolizers.Common.Helpers;
using Monopolizers.Repository.DB;
using Monopolizers.Repository.Models;
using Monopolizers.Repository.Repositories;
using Monopolizers.Service.DTOs;
using Monopolizers.Service.Services;
using System.Security.Claims;

namespace Monopolizers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("SignupAdmin")]
        public async Task<IActionResult> SignUpAdmin(SignUpModel model)
        {
            var result = await _accountService.SignUpWithRoleAsync(model, AppRole.Admin);
            if (!result.Succeeded)
            {
                return StatusCode(500, new ResponseDTO
                {
                    Success = false,
                    Message = string.Join("; ", result.Errors.Select(e => e.Description))
                });
            }

            return Ok(new ResponseDTO
            {
                Success = true,
                Message = "Tạo tài khoản Admin thành công."
            });
        }
        [Authorize(Roles = AppRole.Admin)]
        [HttpPost("SignupManager")]
        public async Task<IActionResult> SignUpManager(SignUpModel model)
        {
            var result = await _accountService.SignUpWithRoleAsync(model, AppRole.Manager);
            if (!result.Succeeded)
            {
                return StatusCode(500, new ResponseDTO
                {
                    Success = false,
                    Message = string.Join("; ", result.Errors.Select(e => e.Description))
                });
            }

            return Ok(new ResponseDTO
            {
                Success = true,
                Message = "Tạo tài khoản Manager thành công."
            });
        }
        [Authorize(Roles = AppRole.Manager)]
        [HttpPost("SignupStaff")]
        public async Task<IActionResult> SignUpStaff(SignUpModel model)
        {
            var result = await _accountService.SignUpWithRoleAsync(model, AppRole.Staff);
            if (!result.Succeeded)
            {
                return StatusCode(500, new ResponseDTO
                {
                    Success = false,
                    Message = string.Join("; ", result.Errors.Select(e => e.Description))
                });
            }

            return Ok(new ResponseDTO
            {
                Success = true,
                Message = "Tạo tài khoản Staff thành công."
            });
        }
        [AllowAnonymous]
        [HttpPost("SignupCustomer")]
        public async Task<IActionResult> SignUpCustomer(SignUpModel model)
        {
            var result = await _accountService.SignUpWithRoleAsync(model, AppRole.Customer);
            if (!result.Succeeded)
            {
                return StatusCode(500, new ResponseDTO
                {
                    Success = false,
                    Message = string.Join("; ", result.Errors.Select(e => e.Description))
                });
            }

            return Ok(new ResponseDTO
            {
                Success = true,
                Message = "Tạo tài khoản Customer thành công."
            });
        }


        [AllowAnonymous]
        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(SignInModel signInModel)
        {
            var result = await _accountService.SignInAsync(signInModel);

            if (result == "Không tìm thấy tài khoản này!" || result == "Sai Mật Khẩu")
            {
                return Unauthorized(new { message = result });
            }

            return Ok(new { token = result });
        }
        
        [HttpPut("BanUser/{userId}")]
        [Authorize(Roles = AppRole.Admin)]

        public async Task<IActionResult> BanUser(string userId)
        {

            var success = await _accountService.BanUserAsync(userId);
            if (!success)
            {
                return NotFound(new ResponseDTO
                {
                    Success = false,
                    Message = "Không tìm thấy người dùng hoặc cập nhật thất bại."
                });
            }

            return Ok(new ResponseDTO
            {
                Success = true,
                Message = "Tài khoản đã bị khóa (Banned)."
            });
        }
        

    }




}
