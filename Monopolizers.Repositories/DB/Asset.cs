using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopolizers.Repository.DB
{
    [Table("Asset")]

    public class Asset
    {
        public int AssetId { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public string AssetType { get; set; }
        public string Name { get; set; }
        public string AccessLevel { get; set; }
        public string SourceUrl { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}
