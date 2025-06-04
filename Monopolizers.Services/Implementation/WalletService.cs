using Monopolizers.Common.DTO;
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

    }
}
