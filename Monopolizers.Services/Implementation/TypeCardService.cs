using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monopolizers.Common.BusinessCode;
using Monopolizers.Common.DTO;
using Monopolizers.Common.DTO.Request;
using Monopolizers.Repository.Contract;
using Monopolizers.Repository.DB;
using Monopolizers.Service.Contract;

namespace Monopolizers.Service.Implementation
{
    public class TypeCardService : ITypeCardService
    {
        private readonly IGenericRepository<TypeCard> _typeCardRepo;
        private readonly IUnitOfWork _unitOfWork;

        public TypeCardService(IGenericRepository<TypeCard> typeCardRepo, IUnitOfWork unitOfWork)
        {
            _typeCardRepo = typeCardRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDTO> CreateAsync(CreateTypeCardDTO dto)
        {
            var type = new TypeCard { Name = dto.Name };
            await _typeCardRepo.Insert(type);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseDTO
            {
                IsSucess = true,
                BusinessCode = BusinessCode.CREATE_SUCCESS,
                message = "TypeCard created successfully",
                Data = type.TypeId
            };
        }

        public async Task<ResponseDTO> DeleteAsync(int id)
        {
            var type = await _typeCardRepo.GetById(id);
            if (type == null)
            {
                return new ResponseDTO
                {
                    IsSucess = false,
                    BusinessCode = BusinessCode.NOT_FOUND,
                    message = "TypeCard not found"
                };
            }
            await _typeCardRepo.DeleteById(id);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseDTO
            {
                IsSucess = true,
                BusinessCode = BusinessCode.UPDATE_SUCESSFULLY,
                message = "TypeCard deleted successfully"
            };
        }

        public async Task<ResponseDTO> GetAllAsync()
        {
            var types = await _typeCardRepo.GetAllDataByExpression(null, 0, 0);
            return new ResponseDTO
            {
                IsSucess = true,
                BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY,
                Data = types.Items.Select(t => new TypeCardDTO { TypeId = t.TypeId, Name = t.Name })
            };
        }

        public async Task<ResponseDTO> UpdateAsync(UpdateTypeCardDTO dto)
        {
            var type = await _typeCardRepo.GetById(dto.TypeId);
            if (type == null)
            {
                return new ResponseDTO
                {
                    IsSucess = false,
                    BusinessCode = BusinessCode.NOT_FOUND,
                    message = "TypeCard not found"
                };
            }
            type.Name = dto.Name;
            await _typeCardRepo.Update(type);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseDTO
            {
                IsSucess = true,
                BusinessCode = BusinessCode.UPDATE_SUCESSFULLY,
                message = "TypeCard updated successfully"
            };
        }
    }
}
