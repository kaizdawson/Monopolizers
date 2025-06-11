using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Monopolizers.Common.DTO.Request;
using Monopolizers.Common.Helpers;
using Monopolizers.Service.Contract;

namespace Monopolizers.API.ManagerController
{
    [Route("api/manager/assets")]
    [ApiController]
    public class AssetManagerController : ControllerBase
    {
        private readonly IAssetService _assetService;

        public AssetManagerController(IAssetService assetService)
        {
            _assetService = assetService;
        }
        [HttpPost("bulk-import")]
        [Authorize(Roles = AppRole.Manager)]
        [RequestSizeLimit(52428800)] // 50MB
        public async Task<IActionResult> BulkImportAssets([FromForm] BulkAssetUploadRequestDTO dto)
        {
            var userId = User.FindFirst("id")?.Value!;
            var res = await _assetService.BulkImportAssetsAsync(dto, userId);
            return Ok(res);
        }
    }
}
