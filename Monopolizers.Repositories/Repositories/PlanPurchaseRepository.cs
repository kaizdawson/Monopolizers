using Monopolizers.Repository.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopolizers.Repository.Repositories
{
    public class PlanPurchaseRepository : IPlanPurchaseRepository
    {
        private readonly CardARContext _context;

        public PlanPurchaseRepository(CardARContext context)
        {
            _context = context;
        }

        public async Task InsertAsync(PlanPurchase purchase)
        {
            await _context.PlanPurchases.AddAsync(purchase);
        }
    }
}
