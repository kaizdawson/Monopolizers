using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopolizers.Repository.DB
{
    [Table("Role")]
    public class Role
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }
        public ICollection<PricingPlans> PricingPlans { get; set; }
    }
}
