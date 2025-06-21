using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Monopolizers.Common.DTO;
using Monopolizers.Common.Enums;
using Monopolizers.Service.Contract;
using Monopolizers.Service.Helpers;
using Monopolizers.Service.Implementation;

namespace Monopolizers.API.UserControllers
{
    [Route("api/customer/cards")]
    [ApiController]
    public class CardCustomerController : ControllerBase
    {
        private readonly ICardService _service;

        public CardCustomerController(ICardService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ResponseDTO> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var access = User.FindFirst("accessLevel")?.Value ?? "Basic";
            Enum.TryParse(access, out AccessLevelEnum level);
            return await _service.GetAllForCustomerAsync(level, pageNumber, pageSize);
        }
        [HttpGet("filter/by-type/{typeId}")]
        public async Task<ResponseDTO> GetCardsByType(int typeId)
        {
            return await _service.GetCardsByTypeAsync(typeId);
        }
        [HttpGet("search")]
        public async Task<ResponseDTO> SearchCards([FromQuery] string keyword, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await _service.SearchCardsAsync(keyword, pageNumber, pageSize);
        }
        [HttpGet("public/all")]
        public async Task<ResponseDTO> GetAllCardsPublic([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await _service.GetAllCardsWithoutAccessFilterAsync(pageNumber, pageSize);
        }
        [HttpGet("check-design/{cardId}")]
        public async Task<ResponseDTO> CheckPermissionToDesign(int cardId)
        {
            var level = AccessLevelHelper.GetAccessLevelFromClaims(User);
            return await _service.CheckDesignPermissionAsync(cardId, level);
        }
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("API OK");
        }
    }
}
