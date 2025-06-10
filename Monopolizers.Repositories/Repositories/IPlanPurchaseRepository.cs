using Monopolizers.Repository.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopolizers.Repository.Repositories
{
    public interface IPlanPurchaseRepository
    {
        Task InsertAsync(PlanPurchase purchase);
    }
}
