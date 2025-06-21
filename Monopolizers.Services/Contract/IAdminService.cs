using Monopolizers.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopolizers.Service.Contract
{
    public interface IAdminService
    {
        Task<ResponseDTO> GetAllUsersAsync();
        Task<ResponseDTO> GetUserByIdAsync(string userId);
        Task<ResponseDTO> UnbanUserAsync(string userId);
        Task<ResponseDTO> GetDashboardOverviewAsync();
        ResponseDTO GetAllRoles();
    }
}
