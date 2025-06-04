using Monopolizers.Common.DTO;
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

    }
}
