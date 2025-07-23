using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopolizers.Common.DTO
{
    public class DashboardStatsDTO
    {
        public DateTime Period { get; set; } // Ngày / Tháng / Năm
        public int NewUsers { get; set; } // Người đăng ký mới
        public int ActiveUsers { get; set; } // Người hoạt động (có giao dịch/mua gói)
        public int PlanPurchases { get; set; } // Tổng lượt mua gói
        public Dictionary<string, int> PlansBought { get; set; } = new(); // Gói mua
        public decimal TotalTopUp { get; set; }
        public decimal TotalPlanRevenue { get; set; }
        public decimal TotalRevenue { get; set; }

    }
}
