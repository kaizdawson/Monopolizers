using Monopolizers.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopolizers.Service.Contract
{
    public interface IPlanService
    {
        Task<IEnumerable<PricingPlanDTO>> GetAllAsync();
        Task<PricingPlanDTO?> GetByIdAsync(int id);
        Task<bool> CreateAsync(CreatePricingPlanDTO dto);
        Task<bool> UpdateAsync(UpdatePricingPlanDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> BuyPlanAsync(string userId, int planId);

        Task<object?> GetCurrentPlanAsync(string userId);

    }
}
