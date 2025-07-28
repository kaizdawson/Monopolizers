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
        public decimal TotalTopUp { get; set; }      // tổng tiền nạp ví
        public int PremiumPlans { get; set; }        // số gói Premium đã mua
        public int VipPlans { get; set; }

    }
}
