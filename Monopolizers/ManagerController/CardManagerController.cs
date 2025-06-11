using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Monopolizers.Common.DTO;
using Monopolizers.Common.DTO.Request;
using Monopolizers.Common.Helpers;
using Monopolizers.Service.Contract;

namespace Monopolizers.API.AdminController
{
    [Authorize(Roles = AppRole.Manager)]
    [Route("api/manager/cards")]
    [ApiController]
    public class CardManagerController : ControllerBase
    {
        private readonly ICardService _service;

        public CardManagerController(ICardService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ResponseDTO> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await _service.GetAllForAdminAsync(pageNumber, pageSize);
        }

        [HttpPost]
        public async Task<ResponseDTO> Create([FromBody] CreateCardDTO dto)
        {
            return await _service.CreateAsync(dto);
        }

        [HttpPut("{id}")]
        public async Task<ResponseDTO> Update(int id, [FromBody] UpdateCardDTO dto)
        {
            return await _service.UpdateAsync(id, dto);
        }

        [HttpDelete("{id}")]
        public async Task<ResponseDTO> Delete(int id)
        {
            return await _service.DeleteAsync(id);
        }
        [HttpGet("{id}")]
        public async Task<ResponseDTO> GetById(int id)
        {
            return await _service.GetByIdAsync(id);
        }
    }
}
