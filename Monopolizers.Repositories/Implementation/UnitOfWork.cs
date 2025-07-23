using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monopolizers.Repository.Contract;
using Monopolizers.Repository.DB;

namespace Monopolizers.Repository.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CardARContext _context;

        public IGenericRepository<PlanPurchase> PlanPurchaseRepository { get; }
        public IGenericRepository<WalletTransaction> WalletTransactionRepository { get; }

        public UnitOfWork(CardARContext context)
        {
            _context = context;
            PlanPurchaseRepository = new GenericRepository<PlanPurchase>(_context);
            WalletTransactionRepository = new GenericRepository<WalletTransaction>(_context);
        }



        public async Task<int> SaveChangeAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
    }
