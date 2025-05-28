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

namespace Monopolizers.Service.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly CardARContext _context;

        public AccountService(IAccountRepository accountRepository, CardARContext context)
        {
            _accountRepository = accountRepository;
            _context = context;
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


    }



}
