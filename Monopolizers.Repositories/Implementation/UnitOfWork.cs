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

        public UnitOfWork(CardARContext context)
        {
            _context = context;
        }
        public async Task<int> SaveChangeAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
    }
