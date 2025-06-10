using Monopolizers.Repository.DB;

namespace Monopolizers.Repository.Repositories
{
    public interface IPricingPlansRepository
    {
        Task<IEnumerable<PricingPlans>> GetAllAsync();
        Task<PricingPlans?> GetByIdAsync(int id);
        Task InsertAsync(PricingPlans entity);
        void Update(PricingPlans entity);
        void Delete(PricingPlans entity);
    }
}
