using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopolizers.Repository.DB
{
    [Table("WalletTransaction")]
    public class WalletTransaction
    {
        [Key]
        public int TransactionId { get; set; }

        [ForeignKey("Wallet")]
        public int WalletId { get; set; }
        public Wallet Wallet { get; set; }

        public decimal Amount { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
