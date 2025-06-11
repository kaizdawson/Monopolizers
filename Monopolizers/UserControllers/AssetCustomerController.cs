using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Monopolizers.Service.Contract;

namespace Monopolizers.API.UserControllers
{
    [Route("api/customer/assets")]
    [ApiController]
    public class AssetCustomerController : ControllerBase
    {
        private readonly IAssetService _service;

        public AssetCustomerController(IAssetService service)
        {
            _service = service;
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var res = await _service.GetAllAssetsAsync();
            return Ok(res);
        }
        [HttpGet("filter")]
        public async Task<IActionResult> FilterAssets([FromQuery] string? theme, [FromQuery] string? assetType)
        {
            var res = await _service.FilterAssetsAsync(theme, assetType);
            return Ok(res);
        }
    }
}
