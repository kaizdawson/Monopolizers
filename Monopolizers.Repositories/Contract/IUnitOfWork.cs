using Monopolizers.Repository.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopolizers.Repository.Contract
{
    public interface IUnitOfWork
    {
        IGenericRepository<PlanPurchase> PlanPurchaseRepository { get; }
        IGenericRepository<WalletTransaction> WalletTransactionRepository { get; }
        public Task<int> SaveChangeAsync();
    }
}
