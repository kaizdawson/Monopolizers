using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopolizers.Repository.DB
{
    [Table("PlanPurchase")]
    public class PlanPurchase
    {
        [Key]
        public int PurchaseId { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [ForeignKey("Plan")]
        public int PlanId { get; set; }
        public PricingPlans Plan { get; set; }

        public decimal Price { get; set; }
        public DateTime PurchasedAt { get; set; }
        public DateTime ExpiredAt { get; set; }
    }
}
