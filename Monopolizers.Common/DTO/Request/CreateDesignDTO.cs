using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopolizers.Common.DTO.Request
{
    public class CreateDesignDTO
    {
        public string DesignName { get; set; }
        public string DesignData { get; set; }
        public string PreviewImageUrl { get; set; }
        public int CardId { get; set; }
    }
}
