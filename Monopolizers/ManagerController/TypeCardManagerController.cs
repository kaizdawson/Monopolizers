using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Monopolizers.Common.DTO;
using Monopolizers.Common.DTO.Request;
using Monopolizers.Common.Helpers;
using Monopolizers.Service.Contract;
using Monopolizers.Service.Implementation;

namespace Monopolizers.API.AdminController
{
    [Authorize(Roles = AppRole.Manager)]
    [Route("api/manager/typecards")]
    [ApiController]
    public class TypeCardManagerController : ControllerBase
    {
        private readonly ITypeCardService _service;

        public TypeCardManagerController(ITypeCardService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ResponseDTO> GetAll()
        {
            return await _service.GetAllAsync();
        }

        [HttpPost]
        public async Task<ResponseDTO> Create([FromBody] CreateTypeCardDTO dto)
        {
            return await _service.CreateAsync(dto);
        }

        [HttpPut]
        public async Task<ResponseDTO> Update([FromBody] UpdateTypeCardDTO dto)
        {
            return await _service.UpdateAsync(dto);
        }

        [HttpDelete("{id}")]
        public async Task<ResponseDTO> Delete(int id)
        {
            return await _service.DeleteAsync(id);
        }
    }
}
