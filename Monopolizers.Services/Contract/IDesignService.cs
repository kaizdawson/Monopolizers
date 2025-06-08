using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monopolizers.Common.DTO;
using Monopolizers.Common.DTO.Request;

namespace Monopolizers.Service.Contract
{
    public interface IDesignService
    {
        Task<ResponseDTO> CreateDesignAsync(CreateDesignDTO dto, string userId);
        Task<ResponseDTO> GetMyDesignsAsync(string userId);
        Task<ResponseDTO> GetDesignByIdAsync(int id, string userId);
        Task<ResponseDTO> DeleteDesignAsync(int id, string userId);
    }
}
