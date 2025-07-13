using System.Threading.Tasks;
using Monopolizers.Repository.DB;

namespace Monopolizers.Repository.Repositories
{
    public interface IWalletTransactionRepository
    {
        Task<bool> ExistsByOrderCodeAsync(string orderCode);
        Task AddAsync(WalletTransaction transaction);
    }
}
