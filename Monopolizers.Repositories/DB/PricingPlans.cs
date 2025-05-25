using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopolizers.Repository.DB
{
    [Table("PricingPlans")]

    public class PricingPlans
    {
        [Key]
        public int PlanId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public bool SupportAR { get; set; }
        public string AccessLevel { get; set; }

        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public Role Role { get; set; }

        public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
        public ICollection<PlanPurchase> PlanPurchases { get; set; }
    }
}
