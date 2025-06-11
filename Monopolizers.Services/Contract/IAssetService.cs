using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monopolizers.Common.DTO;
using Monopolizers.Common.DTO.Request;

namespace Monopolizers.Service.Contract
{
    public interface IAssetService
    {
        Task<ResponseDTO> GetAllAssetsAsync();
        Task<ResponseDTO> BulkImportAssetsAsync(BulkAssetUploadRequestDTO dto, string userId);
        Task<ResponseDTO> FilterAssetsAsync(string? theme, string? assetType);
    }
}
