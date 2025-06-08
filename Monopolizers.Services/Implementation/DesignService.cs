using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Monopolizers.Common.BusinessCode;
using Monopolizers.Common.DTO;
using Monopolizers.Common.DTO.Request;
using Monopolizers.Repository.Contract;
using Monopolizers.Repository.DB;
using Monopolizers.Service.Contract;

namespace Monopolizers.Service.Implementation
{
    public class DesignService : IDesignService
    {
        private readonly IGenericRepository<Design> _designRepo;
        private readonly IUnitOfWork _unitOfWork;

        public DesignService(IGenericRepository<Design> designRepo, IUnitOfWork unitOfWork)
        {
            _designRepo = designRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDTO> CreateDesignAsync(CreateDesignDTO dto, string userId)
        {
            var entity = new Design
            {
                DesignName = dto.DesignName,
                DesignData = dto.DesignData,
                PreviewImageUrl = dto.PreviewImageUrl,
                CardId = dto.CardId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _designRepo.Insert(entity);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseDTO
            {
                IsSucess = true,
                BusinessCode = BusinessCode.CREATE_SUCCESS,
                Data = entity.DesignId,
                message = "Thiết kế đã lưu thành công."
            };
        }

        public async Task<ResponseDTO> DeleteDesignAsync(int id, string userId)
        {
            var design = await _designRepo.GetByExpression(d => d.DesignId == id && d.UserId == userId);
            if (design == null)
            {
                return new ResponseDTO
                {
                    IsSucess = false,
                    BusinessCode = BusinessCode.NOT_FOUND,
                    message = "Không tìm thấy thiết kế cần xoá."
                };
            }

            await _designRepo.DeleteById(id);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseDTO
            {
                IsSucess = true,
                BusinessCode = BusinessCode.UPDATE_SUCESSFULLY,
                message = "Đã xoá thiết kế thành công."
            };
        }

        public async Task<ResponseDTO> GetDesignByIdAsync(int id, string userId)
        {
            var design = await _designRepo.GetByExpression(d => d.DesignId == id && d.UserId == userId);
            if (design == null)
            {
                return new ResponseDTO
                {
                    IsSucess = false,
                    BusinessCode = BusinessCode.NOT_FOUND,
                    message = "Thiết kế không tồn tại."
                };
            }

            return new ResponseDTO
            {
                IsSucess = true,
                BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY,
                Data = design
            };
        }

        public async Task<ResponseDTO> GetMyDesignsAsync(string userId)
        {
            var data = await _designRepo.GetQueryable()
                .Where(d => d.UserId == userId)
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();

            var result = data.Select(d => new DesignDTO
            {
                DesignId = d.DesignId,
                DesignName = d.DesignName,
                PreviewImageUrl = d.PreviewImageUrl,
                CreatedAt = d.CreatedAt,
                CardId = d.CardId
            }).ToList();

            return new ResponseDTO
            {
                IsSucess = true,
                BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY,
                Data = result
            };
        }
    }
}
