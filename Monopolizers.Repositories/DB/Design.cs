using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopolizers.Repository.DB
{
    [Table("Design")]
    public class Design
    {
        public int DesignId { get; set; }
        public string DesignName { get; set; }
        public string DesignData { get; set; }
        public string PreviewImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [ForeignKey("Card")]
        public int CardId { get; set; }
        public Card Card { get; set; }

        public ICollection<Invite> Invites { get; set; }
    }
}
