using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Monopolizers.Repository.DB
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;

        public int? WalletId { get; set; }
        public Wallet Wallet { get; set; } = null!;

        public int? PricingPlansId { get; set; }
        public PricingPlans? PricingPlans { get; set; }

        public string Ban { get; set; } = "InActive";

        public ICollection<Asset>? Assets { get; set; }
        public ICollection<Design>? Designs { get; set; }
        public ICollection<PlanPurchase>? PlanPurchases { get; set; }
    }
}
