using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopolizers.Common.DTO
{
    public class DashboardOverviewDTO
    {
        public List<DashboardStatsDTO> DailyStats { get; set; } = new();
        public List<DashboardStatsDTO> MonthlyStats { get; set; } = new();
        public List<DashboardStatsDTO> YearlyStats { get; set; } = new();
    }
}
