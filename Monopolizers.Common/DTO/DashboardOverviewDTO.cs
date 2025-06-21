using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopolizers.Common.DTO
{
    public class DashboardOverviewDTO
    {
        public Dictionary<string, int> TotalUsersByRole { get; set; } = new();
        public int TotalCards { get; set; }
        public int TotalAssets { get; set; }
        public int TotalSavedCards { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
