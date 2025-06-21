using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Monopolizers.Common.Helpers;
using Monopolizers.Service.Contract;

namespace Monopolizers.API.AdminController
{
    [Route("api/admin")]
    [ApiController]
    [Authorize(Roles = AppRole.Admin)]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }
        // GET: api/admin/users
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var res = await _adminService.GetAllUsersAsync();
            return Ok(res);
        }
        // GET: api/admin/users/{id}
        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var res = await _adminService.GetUserByIdAsync(id);
            return Ok(res);
        }
        // PUT: api/admin/users/unban/{id}
        [HttpPut("users/unban/{id}")]
        public async Task<IActionResult> UnbanUser(string id)
        {
            var res = await _adminService.UnbanUserAsync(id);
            return Ok(res);
        }

        // GET: api/admin/dashboard
        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboardOverview()
        {
            var res = await _adminService.GetDashboardOverviewAsync();
            return Ok(res);
        }

        // GET: api/admin/roles
        [HttpGet("roles")]
        public IActionResult GetRoles()
        {
            var res = _adminService.GetAllRoles();
            return Ok(res);
        }
    }
}
