using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopolizers.Common.DTO
{
    public class PayOSWebhookWrapperDTO
    {
        public string Event { get; set; } 
        public PayOSWebhookDTO Data { get; set; }
    }

}
