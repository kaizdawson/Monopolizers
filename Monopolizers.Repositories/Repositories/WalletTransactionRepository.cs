using Microsoft.EntityFrameworkCore;
using Monopolizers.Repository.DB;
using System;
using System.Threading.Tasks;

namespace Monopolizers.Repository.Repositories
{
    public class WalletTransactionRepository : IWalletTransactionRepository
    {
        private readonly CardARContext _context;

        public WalletTransactionRepository(CardARContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsByOrderCodeAsync(string orderCode)
        {
            if (string.IsNullOrEmpty(orderCode))
                return false;

            return await _context.WalletTransactions
                .AnyAsync(x => x.OrderCode == orderCode);
        }

        public async Task AddAsync(WalletTransaction transaction)
        {
            await _context.WalletTransactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
        }
    }
}
