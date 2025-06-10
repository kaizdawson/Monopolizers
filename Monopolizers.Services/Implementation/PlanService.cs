using Monopolizers.Common.DTO;
using Monopolizers.Repository.DB;
using Monopolizers.Repository.Repositories;
using Monopolizers.Service.Contract;
using Monopolizers.Common.Enums;
using Microsoft.EntityFrameworkCore;
namespace Monopolizers.Service.Implementation
{
    public class PlanService : IPlanService
    {
        private readonly IPricingPlansRepository _repository;
        private readonly CardARContext _context;
        private readonly IWalletRepository _walletRepo;
        private readonly IPlanPurchaseRepository _purchaseRepo;
        public PlanService(CardARContext context,
            IPricingPlansRepository repository,
            IWalletRepository walletRepo,
            IPlanPurchaseRepository purchaseRepo)
        {
            _context = context;
            _repository = repository;
            _walletRepo = walletRepo;
            _purchaseRepo = purchaseRepo;
        }

        public async Task<IEnumerable<PricingPlanDTO>> GetAllAsync()
        {
            var plans = await _repository.GetAllAsync();
            return plans.Select(p => new PricingPlanDTO
            {
                PricingPlansId = p.PricingPlansId,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                SupportAR = p.SupportAR,
                AccessLevel = Enum.Parse<AccessLevelEnum>(p.AccessLevel)
            });
        }

        public async Task<PricingPlanDTO?> GetByIdAsync(int id)
        {
            var p = await _repository.GetByIdAsync(id);
            if (p == null) return null;

            return new PricingPlanDTO
            {
                PricingPlansId = p.PricingPlansId,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                SupportAR = p.SupportAR,
                AccessLevel = Enum.Parse<AccessLevelEnum>(p.AccessLevel)
            };
        }

        public async Task<bool> CreateAsync(CreatePricingPlanDTO dto)
        {
            var entity = new PricingPlans
            {
                Name = dto.Name,
                Price = dto.Price,
                Description = dto.Description,
                SupportAR = dto.SupportAR,
                AccessLevel = dto.AccessLevel.ToString()
            };

            await _repository.InsertAsync(entity);
            return true;
        }

        public async Task<bool> UpdateAsync(UpdatePricingPlanDTO dto)
        {
            var entity = await _repository.GetByIdAsync(dto.PricingPlansId);
            if (entity == null) return false;

            entity.Name = dto.Name;
            entity.Price = dto.Price;
            entity.Description = dto.Description;
            entity.SupportAR = dto.SupportAR;
            entity.AccessLevel = dto.AccessLevel.ToString();

            _repository.Update(entity);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var plan = await _repository.GetByIdAsync(id);
            if (plan == null) return false;

            _repository.Delete(plan);
            return true;
        }

        public async Task<bool> BuyPlanAsync(string userId, int planId)
        {
            var user = await _context.Users.Include(u => u.Wallet).FirstOrDefaultAsync(u => u.Id == userId);
            var plan = await _repository.GetByIdAsync(planId);
            if (user == null || plan == null || user.Wallet == null) return false;
            var activePlan = await _context.PlanPurchases
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.PurchasedAt)
                .FirstOrDefaultAsync();

            if (activePlan != null && activePlan.ExpiredAt > DateTime.UtcNow)
            {
                return false; 
            }
            if (user.Wallet.Balance < plan.Price) return false;

            user.Wallet.Balance -= plan.Price;
            user.PricingPlansId = plan.PricingPlansId;
            _context.Users.Update(user);
            _context.Wallets.Update(user.Wallet);

            var purchase = new PlanPurchase
            {
                UserId = userId,
                PlanId = planId,
                Price = plan.Price,
                PurchasedAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow.AddMonths(1)
            };
            await _purchaseRepo.InsertAsync(purchase);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<object?> GetCurrentPlanAsync(string userId)
        {
            var activePlan = await _context.PlanPurchases
                .Where(p => p.UserId == userId && p.ExpiredAt > DateTime.UtcNow)
                .OrderByDescending(p => p.PurchasedAt)
                .FirstOrDefaultAsync();

            if (activePlan == null) return null;

            var plan = await _repository.GetByIdAsync(activePlan.PlanId);
            if (plan == null) return null;

            return new
            {
                plan.Name,
                plan.Price,
                plan.Description,
                activePlan.PurchasedAt,
                activePlan.ExpiredAt
            };
        }

    }
}