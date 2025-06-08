using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monopolizers.Common.DTO;
using Monopolizers.Common.DTO.Request;
using Monopolizers.Common.Enums;

namespace Monopolizers.Service.Contract
{
    public interface ICardService
    {
        Task<ResponseDTO> GetAllForAdminAsync(int pageNumber, int pageSize);
        Task<ResponseDTO> GetAllForCustomerAsync(AccessLevelEnum userAccessLevel, int pageNumber, int pageSize);
        Task<ResponseDTO> CreateAsync(CreateCardDTO dto);
        Task<ResponseDTO> UpdateAsync(int id, UpdateCardDTO dto);
        Task<ResponseDTO> DeleteAsync(int id);
        Task<ResponseDTO> GetCardsByTypeAsync(int typeId);
        Task<ResponseDTO> SearchCardsAsync(string keyword, int pageNumber, int pageSize);
        Task<ResponseDTO> GetAllCardsWithoutAccessFilterAsync(int pageNumber, int pageSize);
        Task<ResponseDTO> CheckDesignPermissionAsync(int cardId, AccessLevelEnum userLevel);
        Task<ResponseDTO> GetByIdAsync(int id);
    }
}
