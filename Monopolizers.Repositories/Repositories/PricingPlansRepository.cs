using Microsoft.EntityFrameworkCore;
using Monopolizers.Repository.DB;

namespace Monopolizers.Repository.Repositories
{
    public class PricingPlansRepository : IPricingPlansRepository
    {
        private readonly CardARContext _context;

        public PricingPlansRepository(CardARContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PricingPlans>> GetAllAsync()
        {
            return await _context.PricingPlans.ToListAsync();
        }

        public async Task<PricingPlans?> GetByIdAsync(int id)
        {
            return await _context.PricingPlans.FindAsync(id);
        }

        public async Task InsertAsync(PricingPlans entity)
        {
            await _context.PricingPlans.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public void Update(PricingPlans entity)
        {
            _context.PricingPlans.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(PricingPlans entity)
        {
            _context.PricingPlans.Remove(entity);
            _context.SaveChanges();
        }
    }
}
