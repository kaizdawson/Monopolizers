using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Monopolizers.Common.DTO;
using Monopolizers.Service.Contract;

namespace Monopolizers.API.UserControllers
{
    [Route("api/customer/saved-cards")]
    [ApiController]
    public class SavedCardController : ControllerBase
    {
        private readonly ISavedCardService _service;

        public SavedCardController(ISavedCardService service)
        {
            _service = service;
        }
        [HttpPost]
        public async Task<IActionResult> SaveCard([FromBody] SaveCardDTO dto)
        {
            var userId = User.FindFirst("id")?.Value!;
            var res = await _service.SaveCardAsync(dto, userId);
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetMyCards()
        {
            var userId = User.FindFirst("id")?.Value!;
            var res = await _service.GetMySavedCardsAsync(userId);
            return Ok(res);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = User.FindFirst("id")?.Value!;
            var res = await _service.GetSavedCardByIdAsync(id, userId);
            return Ok(res);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirst("id")?.Value!;
            var res = await _service.DeleteSavedCardAsync(id, userId);
            return Ok(res);
        }
        [AllowAnonymous]
        [HttpGet("view/{id}")]
        public async Task<IActionResult> GetPublicById(int id)
        {
            var res = await _service.GetSavedCardByIdForPublicAsync(id);
            return Ok(res);
        }

    }
}
