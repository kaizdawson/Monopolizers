using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopolizers.Common.DTO
{
    public class PayOSWebhookDTO
    {
        public long orderCode { get; set; }
        public int amount { get; set; } 
        public string description { get; set; }
        public string status { get; set; } 
        public string transactionId { get; set; }
        public long time { get; set; } 
    }

}
