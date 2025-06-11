using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopolizers.Common.DTO.Request
{
    public class BulkAssetUploadRequestDTO
    {
        public List<AssetUploadDTO> Assets { get; set; } = new();
    }
}
