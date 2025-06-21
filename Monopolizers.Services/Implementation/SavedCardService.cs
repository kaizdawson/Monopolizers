using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Monopolizers.Common.BusinessCode;
using Monopolizers.Common.DTO;
using Monopolizers.Repository.Contract;
using Monopolizers.Repository.DB;
using Monopolizers.Service.Contract;

namespace Monopolizers.Service.Implementation
{
    public class SavedCardService : ISavedCardService
    {
        private readonly IGenericRepository<UserSavedCard> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public SavedCardService(IGenericRepository<UserSavedCard> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDTO> DeleteSavedCardAsync(int id, string userId)
        {
            var item = await _repository.GetByExpression(x => x.Id == id && x.UserId == userId);
            if (item == null)
            {
                return new ResponseDTO
                {
                    IsSucess = false,
                    BusinessCode = BusinessCode.NOT_FOUND,
                    message = "Không tìm thấy thiệp cần xoá."
                };
            }

            await _repository.DeleteById(id);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseDTO
            {
                IsSucess = true,
                BusinessCode = BusinessCode.UPDATE_SUCESSFULLY,
                message = "Xoá thiệp thành công."
            };
        }

        public async Task<ResponseDTO> GetMySavedCardsAsync(string userId)
        {
            var data = await _repository.GetQueryable()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.SavedAt)
                .ToListAsync();
            var result = data.Select(x => new UserSavedCardDTO
            {
                Id = x.Id,
                CardId = x.CardId,
                Background = x.Background,
                ElementsJson = x.ElementsJson,
                SavedAt = x.SavedAt
            }).ToList();
            return new ResponseDTO
            {
                IsSucess = true,
                BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY,
                Data = result
            };
        
        }

        public async Task<ResponseDTO> GetSavedCardByIdAsync(int id, string userId)
        {
            var item = await _repository.GetByExpression(x => x.Id == id && x.UserId == userId);
            if (item == null)
            {
                return new ResponseDTO
                {
                    IsSucess = false,
                    BusinessCode = BusinessCode.NOT_FOUND,
                    message = "Không tìm thấy thiệp."
                };
            }

            var dto = new UserSavedCardDTO
            {
                Id = item.Id,
                CardId = item.CardId,
                Background = item.Background,
                ElementsJson = item.ElementsJson,
                SavedAt = item.SavedAt
            };

            return new ResponseDTO
            {
                IsSucess = true,
                BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY,
                Data = dto
            };
        }

        public async Task<ResponseDTO> GetSavedCardByIdForPublicAsync(int id)
        {
          var item = await _repository.GetById(id);
            if (item == null)
            {
                return new ResponseDTO
                {
                    IsSucess = false,
                    BusinessCode = BusinessCode.NOT_FOUND,
                    message = "Không tìm thấy thiệp."
                };
            }
            var dto = new UserSavedCardDTO
            {
                Id = item.Id,
                CardId = item.CardId,
                Background = item.Background,
                ElementsJson = item.ElementsJson,
                SavedAt = item.SavedAt
            };
            return new ResponseDTO
            {
                IsSucess = true,
                BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY,
                Data = dto
            };
        }

        public async Task<ResponseDTO> SaveCardAsync(SaveCardDTO dto, string userId)
        {
            var entity = new UserSavedCard
            {
                CardId = dto.CardId,
                Background = dto.Background,
                ElementsJson = dto.ElementsJson,
                UserId = userId,
                SavedAt = DateTime.UtcNow
            };
            await _repository.Insert(entity);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseDTO
            {
                IsSucess = true,
                BusinessCode = BusinessCode.CREATE_SUCCESS,
                message = "Thiệp đã được lưu thành công.",
                Data = new { id = entity.Id }
            };
        }
    }
}
