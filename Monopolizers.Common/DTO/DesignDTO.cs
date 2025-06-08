using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopolizers.Common.DTO
{
    public class DesignDTO
    {
        public int DesignId { get; set; }
        public string DesignName { get; set; }
        public string PreviewImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CardId { get; set; }
    }
}
