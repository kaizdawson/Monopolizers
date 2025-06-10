using Monopolizers.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopolizers.Common.DTO
{
    public class PricingPlanDTO
    {
        public int PricingPlansId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public bool SupportAR { get; set; }
        public AccessLevelEnum AccessLevel { get; set; }
    }
}
