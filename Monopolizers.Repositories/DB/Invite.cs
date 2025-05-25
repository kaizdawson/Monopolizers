using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopolizers.Repository.DB
{
    [Table("Invite")]
    public class Invite
    {
        public int InviteId { get; set; }

        [ForeignKey("Design")]
        public int DesignId { get; set; }
        public Design Design { get; set; }

        public string EmailReceiver { get; set; }
        public string QrCodeUrl { get; set; }
        public DateTime SentAt { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
