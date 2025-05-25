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

namespace Monopolizers.Service.Services
{
        public class AccountService : IAccountService
        {
            private readonly IAccountRepository _accountRepository;
            private readonly CardARContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountService(IAccountRepository accountRepository, CardARContext context, UserManager<ApplicationUser> userManager)
            {
                _accountRepository = accountRepository;
                _context = context;
                _userManager = userManager;
        }

        public async Task<IdentityResult> SignUpAsync(SignUpModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                RoleId = 1 //Customer
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
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
            }

            return result;
        }

        public async Task<string> SignInAsync(SignInModel model)
            {
                return await _accountRepository.SignInAsync(model);
            }
        }

}
