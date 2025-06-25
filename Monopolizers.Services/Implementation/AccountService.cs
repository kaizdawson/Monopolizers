using Microsoft.AspNetCore.Identity;
using Monopolizers.Repository.DB;
using Monopolizers.Repository.Models;
using Monopolizers.Repository.Repositories;
using Monopolizers.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Monopolizers.Common.Helpers;
using Monopolizers.Common.DTO;
using ResponseDTO = Monopolizers.Common.DTO.ResponseDTO;
using Monopolizers.Service.Contract;
using Monopolizers.Common.DTO.Request;
using System.Web;

namespace Monopolizers.Service.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly CardARContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;

        public AccountService(IAccountRepository accountRepository, CardARContext context, UserManager<ApplicationUser> userManager, IEmailService emailService)
        {
            _accountRepository = accountRepository;
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task<IdentityResult> SignUpWithRoleAsync(SignUpModel model, string roleName)
        {
           
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                Ban = "InActive"
            };

          
            var result = await _accountRepository.CreateUserAsync(user, model.Password);
            if (!result.Succeeded) return result;

            
            var wallet = new Wallet
            {
                Balance = 0,
                User = user
            };
            _context.Wallets.Add(wallet);
            await _context.SaveChangesAsync();

            
            user.WalletId = wallet.WalletId;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            
            await _accountRepository.AddUserToRoleAsync(user, roleName);

            return result;
        }

        public async Task<string> SignInAsync(SignInModel model)
        {
            return await _accountRepository.SignInAsync(model);
        }

        public async Task<bool> BanUserAsync(string userId)
        {
            return await _accountRepository.BanUserAsync(userId);
        }

        public async Task<SimpleResponseDTO> ForgotPasswordAsync(ForgotPasswordRequestDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return new SimpleResponseDTO(false, "Email không tồn tại");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = $"https://monopolizers.vercel.app/reset-password?email={dto.Email}&token={HttpUtility.UrlEncode(token)}";

            await _emailService.SendEmailAsync(new EmailDTO
            {
                To = dto.Email,
                Subject = "Reset your password",
                Body = $"<p>Click link để reset mật khẩu:</p><a href='{resetLink}'>Đặt lại mật khẩu</a>"
            });

            return new SimpleResponseDTO(true, "Đã gửi email đặt lại mật khẩu.");
        }

        public async Task<SimpleResponseDTO> ResetPasswordAsync(ResetPasswordRequestDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return new SimpleResponseDTO(false, "Email không tồn tại");

            // ✅ FIX TẠI ĐÂY:
            var decodedToken = HttpUtility.UrlDecode(dto.Token);

            var result = await _userManager.ResetPasswordAsync(user, decodedToken, dto.NewPassword);

            if (result.Succeeded)
                return new SimpleResponseDTO(true, "Đặt lại mật khẩu thành công");

            return new SimpleResponseDTO(false, "Token không hợp lệ hoặc đã hết hạn");
        }
    }



}
