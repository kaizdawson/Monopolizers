using Monopolizers.Common.DTO;
using Monopolizers.Repository.Repositories;
using Monopolizers.Service.Contract;
using Monopolizers.Repository.Repositories;
using Monopolizers.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopolizers.Service.Implementation
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _walletRepository;

        public WalletService(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        public async Task<WalletDTO> GetWalletBalanceAsync(string userId)
        {
            return await _walletRepository.GetWalletBalanceAsync(userId);
        }

        public async Task UpdateWalletBalanceAsync(WalletDTO walletDTO)
        {
            await _walletRepository.UpdateWalletBalanceAsync(walletDTO);
        }

        public async Task<string> DepositAsync(string userId, decimal amount)
        {
            return await _walletRepository.DepositAsync(userId, amount);
        }

        public async Task AddBalanceFromPayOSAsync(string userId, decimal amount)
        {
            var walletId = await _walletRepository.GetWalletIdByUserIdAsync(userId);
            if (walletId == null) throw new Exception("Wallet not found for user");

            var wallet = await _walletRepository.GetByIdAsync(walletId.Value);
            if (wallet == null) throw new Exception("Wallet not found");

            wallet.Balance += amount;
            await _walletRepository.UpdateAsync(wallet);
        }



    }
}