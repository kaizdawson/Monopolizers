using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopolizers.Common.DTO
{
    public class AssetDTO
    {
        public int AssetId { get; set; }
        public string Name { get; set; }
        public string AssetType { get; set; }
        public string SourceUrl { get; set; }
        public string AccessLevel { get; set; }
        public string Theme { get; set; } = null!;
    }
}
