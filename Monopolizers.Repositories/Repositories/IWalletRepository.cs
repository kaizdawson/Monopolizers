using Monopolizers.Common.DTO;
using Monopolizers.Repository.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopolizers.Repository.Repositories
{
    public interface IWalletRepository
    {
        Task<WalletDTO> GetWalletBalanceAsync(string userId);
        Task UpdateWalletBalanceAsync(WalletDTO walletDTO);

        Task<string> DepositAsync(string userId, decimal amount);
        Task<Wallet> GetByIdAsync(int walletId);
        Task UpdateAsync(Wallet wallet);
        Task<int?> GetWalletIdByUserIdAsync(string userId);

    }
}
