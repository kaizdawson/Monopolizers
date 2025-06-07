using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Monopolizers.Common.DTO.Request;
using Monopolizers.Service.Contract;

namespace Monopolizers.API.UserControllers
{
    [Route("api/customer/designs")]
    [ApiController]
    public class DesignCustomerController : ControllerBase
    {
        private readonly IDesignService _service;

        public DesignCustomerController(IDesignService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDesignDTO dto)
        {
            var userId = User.FindFirst("id")?.Value!;
            var res = await _service.CreateDesignAsync(dto, userId);
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetMine()
        {
            var userId = User.FindFirst("id")?.Value!;
            var res = await _service.GetMyDesignsAsync(userId);
            return Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = User.FindFirst("id")?.Value!;
            var res = await _service.GetDesignByIdAsync(id, userId);
            return Ok(res);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirst("id")?.Value!;
            var res = await _service.DeleteDesignAsync(id, userId);
            return Ok(res);
        }
    }
}
