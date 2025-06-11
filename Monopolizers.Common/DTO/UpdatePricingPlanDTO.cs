using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopolizers.Common.DTO
{
    public class UpdatePricingPlanDTO : CreatePricingPlanDTO
    {
        public int PricingPlansId { get; set; }
    }
}
