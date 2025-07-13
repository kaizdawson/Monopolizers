using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopolizers.Common.DTO
{
    public class PayOSWebhookDTO
    {
        public int OrderCode { get; set; }
        public int Amount { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public long TransactionId { get; set; }
        public long Time { get; set; }
    }
}
