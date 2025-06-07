using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monopolizers.Common.DTO;

namespace Monopolizers.Service.Contract
{
    public interface ISavedCardService
    {
        Task<ResponseDTO> SaveCardAsync(SaveCardDTO dto, string userId);
        Task<ResponseDTO> GetMySavedCardsAsync(string userId);
        Task<ResponseDTO> GetSavedCardByIdAsync(int id, string userId);
        Task<ResponseDTO> DeleteSavedCardAsync(int id, string userId);
    }
}

