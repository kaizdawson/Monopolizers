using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monopolizers.Common.DTO;
using Monopolizers.Common.DTO.Request;

namespace Monopolizers.Service.Contract
{
    public interface ITypeCardService
    {
        Task<ResponseDTO> GetAllAsync();
        Task<ResponseDTO> CreateAsync(CreateTypeCardDTO dto);
        Task<ResponseDTO> UpdateAsync(UpdateTypeCardDTO dto);
        Task<ResponseDTO> DeleteAsync(int id);

    }
}
