using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Monopolizers.Common.DTO
{
    public class AssetUploadDTO
    {
        public IFormFile File { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string AssetType { get; set; } = null!; // "image", "text"
        public string AccessLevel { get; set; } = null!; // "Basic", "Premium", "VIP"
        public string Theme { get; set; } = null!;
    }
}
