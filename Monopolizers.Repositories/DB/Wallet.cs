using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopolizers.Repository.DB
{
    [Table("Wallet")]
    public class Wallet
    {
        [Key]
        public int WalletId { get; set; }
        public decimal Balance { get; set; }
        public ApplicationUser User { get; set; }

        public ICollection<WalletTransaction>? WalletTransactions { get; set; } = new List<WalletTransaction>();
    }
}
