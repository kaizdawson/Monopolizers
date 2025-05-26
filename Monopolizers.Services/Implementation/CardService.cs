using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monopolizers.Common.BusinessCode;
using Monopolizers.Common.DTO;
using Monopolizers.Common.DTO.Request;
using Monopolizers.Common.Enums;
using Monopolizers.Repository.Contract;
using Monopolizers.Repository.DB;
using Monopolizers.Service.Contract;

namespace Monopolizers.Service.Implementation
{
    public class CardService : ICardService
    {
        private readonly IGenericRepository<Card> _cardRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CardService(IGenericRepository<Card> cardRepository, IUnitOfWork unitOfWork)
        {
            _cardRepository = cardRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDTO> CheckDesignPermissionAsync(int cardId, AccessLevelEnum userLevel)// hàm này để check gói khách hàng có phải đã mua gói chưa để vào design
        {
            ResponseDTO dto = new ResponseDTO();
            try
            {
                var card = await _cardRepository.GetById(cardId);
                if (card == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.NOT_FOUND;
                    dto.message = "Card not found.";
                    return dto;
                }

                var cardLevel = Enum.Parse<AccessLevelEnum>(card.AccessLevel);
                bool canDesign = userLevel == AccessLevelEnum.VIP ||
                                 (userLevel == AccessLevelEnum.Premium && cardLevel != AccessLevelEnum.VIP) ||
                                 cardLevel == AccessLevelEnum.Basic;

                if (!canDesign)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.EXCEPTION;
                    dto.message = $"You don't have permission to design this card. Required level: {card.AccessLevel}";
                    return dto;
                }

                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Data = new CardDTO
                {
                    CardId = card.CardId,
                    Title = card.Title,
                    Description = card.Description,
                    AvtImgUrl = card.AvtImgUrl,
                    ARVideoUrl = card.ARVideoUrl,
                    Category = card.Category,
                    TypeId = card.TypeId,
                    DefaultData = card.DefaultData,
                    AccessLevel = cardLevel
                };
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.message = ex.Message;
            }
            return dto;
        
        }

        public async Task<ResponseDTO> CreateAsync(CreateCardDTO dto)
        {
            ResponseDTO res = new ResponseDTO();

            if (dto == null)
            {
                res.IsSucess = false;
                res.BusinessCode = BusinessCode.INVALID_INPUT;
                res.message = "Request body is null or invalid (dto == null)";
                return res;
            }

            try
            {
                var card = new Card
                {
                    Title = dto.Title,
                    Description = dto.Description,
                    AvtImgUrl = dto.AvtImgUrl,
                    ARVideoUrl = dto.ARVideoUrl,
                    Category = dto.Category,
                    TypeId = dto.TypeId,
                    DefaultData = dto.DefaultData,
                    AccessLevel = dto.AccessLevel.ToString()
                };

                await _cardRepository.Insert(card);
                await _unitOfWork.SaveChangeAsync();

                res.IsSucess = true;
                res.BusinessCode = BusinessCode.CREATE_SUCCESS;
                res.Data = card.CardId;
                res.message = "Card created successfully.";
            }
            catch (Exception ex)
            {
                res.IsSucess = false;
                res.BusinessCode = BusinessCode.EXCEPTION;
                res.message = ex.Message;
            }

            return res;
        }

        public async Task<ResponseDTO> DeleteAsync(int id)
        {
            ResponseDTO dto = new ResponseDTO();
            try
            {
                var card = await _cardRepository.GetById(id);
                if (card == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.NOT_FOUND;
                    dto.message = "Card not found.";
                    return dto;
                }
                await _cardRepository.DeleteById(id);
                await _unitOfWork.SaveChangeAsync();

                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.UPDATE_SUCESSFULLY;
                dto.message = "Card deleted successfully.";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.message = ex.Message;
            }
            return dto;
        }

        public async Task<ResponseDTO> GetAllForAdminAsync(int pageNumber, int pageSize)
        {
            ResponseDTO dto = new ResponseDTO();
            try
            {
                var cards = await _cardRepository.GetAllDataByExpression(null, pageNumber, pageSize);
                dto.Data = cards.Items.Select(c => new CardDTO
                {
                    CardId = c.CardId,
                    Title = c.Title,
                    Description = c.Description,
                    AvtImgUrl = c.AvtImgUrl,
                    ARVideoUrl = c.ARVideoUrl,
                    Category = c.Category,
                    TypeId = c.TypeId,
                    DefaultData = c.DefaultData,
                    AccessLevel = Enum.Parse<AccessLevelEnum>(c.AccessLevel)
                }).ToList();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.message = ex.Message;
            }
            return dto;
        }
        public async Task<ResponseDTO> GetAllCardsWithoutAccessFilterAsync(int pageNumber, int pageSize) // hàm này là get all card show ở homepage cho customer
        {
            ResponseDTO dto = new ResponseDTO();
            try
            {
                var cards = await _cardRepository.GetAllDataByExpression(null, pageNumber, pageSize);

                dto.Data = cards.Items.Select(c => new CardDTO
                {
                    CardId = c.CardId,
                    Title = c.Title,
                    Description = c.Description,
                    AvtImgUrl = c.AvtImgUrl,
                    ARVideoUrl = c.ARVideoUrl,
                    Category = c.Category,
                    TypeId = c.TypeId,
                    DefaultData = c.DefaultData,
                    AccessLevel = Enum.Parse<AccessLevelEnum>(c.AccessLevel)
                }).ToList();

                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.message = ex.Message;
            }
            return dto;
        }

        // hàm này để sử dụng “Kho mẫu của tôi” không phải hàm getall cho customer chủ yếu để check người dùng sử dụng gói gì để show card
        public async Task<ResponseDTO> GetAllForCustomerAsync(AccessLevelEnum userAccessLevel, int pageNumber, int pageSize)
        {
            ResponseDTO dto = new ResponseDTO();
            try
            {
                var cards = await _cardRepository.GetAllDataByExpression(
                    c => c.AccessLevel == AccessLevelEnum.Basic.ToString() ||
                         userAccessLevel == AccessLevelEnum.VIP ||
                         (userAccessLevel == AccessLevelEnum.Premium && c.AccessLevel != AccessLevelEnum.VIP.ToString()),
                    pageNumber, pageSize);

                dto.Data = cards.Items.Select(c => new CardDTO
                {
                    CardId = c.CardId,
                    Title = c.Title,
                    Description = c.Description,
                    AvtImgUrl = c.AvtImgUrl,
                    ARVideoUrl = c.ARVideoUrl,
                    Category = c.Category,
                    TypeId = c.TypeId,
                    DefaultData = c.DefaultData,
                    AccessLevel = Enum.Parse<AccessLevelEnum>(c.AccessLevel)
                }).ToList();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.message = ex.Message;
            }
            return dto;
        }

        public async Task<ResponseDTO> GetCardsByTypeAsync(int typeId)
        {
            ResponseDTO dto = new ResponseDTO();
            try
            {
                var cards = await _cardRepository.GetAllDataByExpression(
                    c => c.TypeId == typeId,
                    0, 0);

                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Data = cards.Items.Select(c => new CardDTO
                {
                    CardId = c.CardId,
                    Title = c.Title,
                    Description = c.Description,
                    AvtImgUrl = c.AvtImgUrl,
                    ARVideoUrl = c.ARVideoUrl,
                    Category = c.Category,
                    TypeId = c.TypeId,
                    DefaultData = c.DefaultData,
                    AccessLevel = Enum.Parse<AccessLevelEnum>(c.AccessLevel)
                }).ToList();
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.message = ex.Message;
            }
            return dto;
        }

        public async Task<ResponseDTO> SearchCardsAsync(string keyword, int pageNumber, int pageSize)
        {
            ResponseDTO dto = new ResponseDTO();
            try
            {
                keyword = keyword.ToLower();

                var cards = await _cardRepository.GetAllDataByExpression(
                    c =>
                        c.Title.ToLower().Contains(keyword) ||
                        c.Description.ToLower().Contains(keyword),
                    pageNumber, pageSize);

                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Data = cards.Items.Select(c => new CardDTO
                {
                    CardId = c.CardId,
                    Title = c.Title,
                    Description = c.Description,
                    AvtImgUrl = c.AvtImgUrl,
                    ARVideoUrl = c.ARVideoUrl,
                    Category = c.Category,
                    TypeId = c.TypeId,
                    DefaultData = c.DefaultData,
                    AccessLevel = Enum.Parse<AccessLevelEnum>(c.AccessLevel)
                }).ToList();
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.message = ex.Message;
            }
            return dto;
        }

        public async Task<ResponseDTO> UpdateAsync(int id, UpdateCardDTO dto)
        {
            ResponseDTO res = new ResponseDTO();
            try
            {
                var card = await _cardRepository.GetById(id);
                if (card == null)
                {
                    res.IsSucess = false;
                    res.BusinessCode = BusinessCode.NOT_FOUND;
                    res.message = "Card not found.";
                    return res;
                }

                card.Title = dto.Title;
                card.Description = dto.Description;
                card.AvtImgUrl = dto.AvtImgUrl;
                card.ARVideoUrl = dto.ARVideoUrl;
                card.Category = dto.Category;
                card.TypeId = dto.TypeId;
                card.DefaultData = dto.DefaultData;
                card.AccessLevel = dto.AccessLevel.ToString();

                await _cardRepository.Update(card);
                await _unitOfWork.SaveChangeAsync();

                res.IsSucess = true;
                res.BusinessCode = BusinessCode.UPDATE_SUCESSFULLY;
                res.Data = card.CardId;
                res.message = "Card updated successfully.";
            }
            catch (Exception ex)
            {
                res.IsSucess = false;
                res.BusinessCode = BusinessCode.EXCEPTION;
                res.message = ex.Message;
            }
            return res;
        }
    }
}
