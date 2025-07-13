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
using Monopolizers.Repository.DB;
using Microsoft.Extensions.Logging;

namespace Monopolizers.Service.Implementation
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IWalletTransactionRepository _walletTransactionRepository;
        private readonly ILogger<WalletService> _logger;

        public WalletService(
            IWalletRepository walletRepository,
            IWalletTransactionRepository walletTransactionRepository,
            ILogger<WalletService> logger)
        {
            _walletRepository = walletRepository;
            _walletTransactionRepository = walletTransactionRepository;
            _logger = logger;
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

        public async Task AddBalanceFromPayOSAsync(string userId, decimal amount, string orderCode)
        {
            _logger?.LogInformation($"▶️ Kiểm tra giao dịch với OrderCode = {orderCode}");
            bool exists = await _walletTransactionRepository.ExistsByOrderCodeAsync(orderCode);
            if (exists)
            {
                _logger?.LogWarning($"⚠️ Đã tồn tại OrderCode = {orderCode}, bỏ qua cộng tiền");
                return;
            }


            var walletId = await _walletRepository.GetWalletIdByUserIdAsync(userId);
            if (walletId == null) throw new Exception("Wallet not found for user");

            var wallet = await _walletRepository.GetByIdAsync(walletId.Value);
            if (wallet == null) throw new Exception("Wallet not found");

            wallet.Balance += amount;
            await _walletRepository.UpdateAsync(wallet);

            // ✅ Lưu giao dịch vào WalletTransaction
            var transaction = new WalletTransaction
            {
                WalletId = wallet.WalletId,
                Amount = amount,
                Type = "Deposit",
                Description = $"Nạp từ PayOS - OrderCode: {orderCode}",
                OrderCode = orderCode,
                CreatedAt = DateTime.UtcNow
            };

            await _walletTransactionRepository.AddAsync(transaction);
        }
    



}
}