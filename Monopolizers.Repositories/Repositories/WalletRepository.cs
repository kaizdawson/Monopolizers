using Microsoft.EntityFrameworkCore;
using Monopolizers.Common.DTO;
using Monopolizers.Repository.DB;

namespace Monopolizers.Repository.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly CardARContext _context;

        public WalletRepository(CardARContext context)
        {
            _context = context;
        }

        public async Task<WalletDTO> GetWalletBalanceAsync(string userId)
        {
            var user = await _context.Users
                .Include(u => u.Wallet)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user?.Wallet == null)
                return null;

            return new WalletDTO
            {
                WalletId = user.Wallet.WalletId,
                Balance = (decimal)user.Wallet.Balance
            };
        }

        public async Task UpdateWalletBalanceAsync(WalletDTO walletDTO)
        {
            var existingEntity = await _context.Wallets.FindAsync(walletDTO.WalletId);

            if (existingEntity == null)
                throw new KeyNotFoundException($"Không tìm thấy ví với ID {walletDTO.WalletId}");

          
            existingEntity.Balance = walletDTO.Balance;

            _context.Wallets.Update(existingEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<string> DepositAsync(string userId, decimal amount)
        {
            var user = await _context.Users
                .Include(u => u.Wallet)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return "Không tìm thấy người dùng.";

            if (user.Wallet == null)
                return "Người dùng chưa có ví.";

            user.Wallet.Balance += amount;

            var transaction = new WalletTransaction
            {
                WalletId = user.Wallet.WalletId,
                Amount = amount,
                Type = "Deposit",
                Description = "Nạp tiền qua VNPay",
                CreatedAt = DateTime.UtcNow
            };

            _context.WalletTransactions.Add(transaction);
            await _context.SaveChangesAsync();

            return "Success";

        }

    }
}
