using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Monopolizers.Common.DTO;
using Monopolizers.Common.Helpers;
using Monopolizers.Repository.DB;
using Monopolizers.Service.Contract;
using Monopolizers.Service.DTOs;
using Monopolizers.Service.Implementation;
using System.Security.Claims;

namespace Monopolizers.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanController : ControllerBase
    {
        private readonly IPlanService _planService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtService _jwtService;
        public PlanController(IPlanService planService, UserManager<ApplicationUser> userManager, JwtService jwtService)
        {
            _planService = planService;
            _userManager = userManager;
            _jwtService = jwtService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _planService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _planService.GetByIdAsync(id);
            if (result == null) return NotFound("Gói không tồn tại.");
            return Ok(result);
        }
        [Authorize(Roles = AppRole.Manager)]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreatePricingPlanDTO dto)
        {
            var success = await _planService.CreateAsync(dto);
            return success ? Ok("Tạo thành công") : BadRequest("Tạo thất bại");
        }
        [Authorize(Roles = AppRole.Manager)]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdatePricingPlanDTO dto)
        {
            var success = await _planService.UpdateAsync(dto);
            return success ? Ok("Cập nhật thành công") : NotFound("Không tìm thấy gói để cập nhật");
        }
        [Authorize(Roles = AppRole.Manager)]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _planService.DeleteAsync(id);
            return success ? Ok("Xoá thành công") : NotFound("Không tìm thấy gói để xoá");
        }

        [HttpPost("buy")]
        [Authorize(Roles = AppRole.Customer)]
        public async Task<IActionResult> BuyPlan([FromBody] BuyPlanRequestDTO dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var success = await _planService.BuyPlanAsync(userId, dto.PlanId);
            if (!success)
                return BadRequest("Không đủ số dư hoặc đang còn thời hạn ở gói khác!");

            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);

            var accessToken = _jwtService.GenerateAccessToken(user, roles);

            return Ok(new
            {
                message = "Mua gói thành công",
                accessToken
            });
        }


        [HttpGet("current")]
        [Authorize(Roles = AppRole.Customer)]
        public async Task<IActionResult> GetCurrentPlan()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var result = await _planService.GetCurrentPlanAsync(userId);
            return result != null ? Ok(result) : NotFound("Bạn chưa có gói nào đang sử dụng");
        }

    }
}
