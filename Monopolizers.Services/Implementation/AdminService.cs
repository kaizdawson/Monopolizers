using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Monopolizers.Common.BusinessCode;
using Monopolizers.Common.Constants;
using Monopolizers.Common.DTO;
using Monopolizers.Repository.Contract;
using Monopolizers.Repository.DB;
using Monopolizers.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Monopolizers.Service.Implementation
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGenericRepository<Card> _cardRepository;
        private readonly IGenericRepository<Asset> _assetRepository;
        private readonly IGenericRepository<UserSavedCard> _savedCardRepository;
        private readonly IGenericRepository<WalletTransaction> _walletTransactionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AdminService(UserManager<ApplicationUser> userManager, IGenericRepository<Card> cardRepository, IGenericRepository<Asset> assetRepository, IGenericRepository<UserSavedCard> savedCardRepository, IGenericRepository<WalletTransaction> walletTransactionRepository, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _cardRepository = cardRepository;
            _assetRepository = assetRepository;
            _savedCardRepository = savedCardRepository;
            _walletTransactionRepository = walletTransactionRepository;
            _unitOfWork = unitOfWork;
        }

        public ResponseDTO GetAllRoles()
        {
            return new ResponseDTO
            {
                IsSucess = true,
                BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY,
                Data = new List<string> { "Admin", "Manager", "Staff", "Customer" }
            };
        }

        public async Task<ResponseDTO> GetAllUsersAsync()
        {
            var res = new ResponseDTO();
            try
            {
                var users = await Task.FromResult(_userManager.Users.ToList());
                var data = new List<UserAdminDTO>();

                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    data.Add(new UserAdminDTO
                    {
                        UserId = user.Id,
                        Email = user.Email ?? "",
                        FullName = $"{user.FirstName} {user.LastName}",
                        Roles = roles.ToList(),
                        BanStatus = user.Ban
                    });
                }

                res.IsSucess = true;
                res.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                res.Data = data;
            }
            catch (Exception ex)
            {
                res.IsSucess = false;
                res.BusinessCode = BusinessCode.EXCEPTION;
                res.message = ex.Message;
            }
            return res;
        }

        public async Task<ResponseDTO> GetDashboardOverviewAsync()
        {
            var res = new ResponseDTO();
            try
            {
                var totalUsersByRole = new Dictionary<string, int>
                {
                    { "Admin", (await _userManager.GetUsersInRoleAsync("Admin")).Count },
                    { "Manager", (await _userManager.GetUsersInRoleAsync("Manager")).Count },
                    { "Staff", (await _userManager.GetUsersInRoleAsync("Staff")).Count },
                    { "Customer", (await _userManager.GetUsersInRoleAsync("Customer")).Count }
                };

                var totalCards = await _cardRepository.GetQueryable().CountAsync();
                var totalAssets = await _assetRepository.GetQueryable().CountAsync();
                var totalSavedCards = await _savedCardRepository.GetQueryable().CountAsync();
                var totalRevenue = await _walletTransactionRepository
                    .GetQueryable()
                    .Where(t => t.Type == TransactionTypes.Deposit || t.Type == TransactionTypes.TopUp || t.Type == TransactionTypes.BuyPlan)
                    .SumAsync(t => t.Amount);

                var dashboard = new DashboardOverviewDTO
                {
                    TotalUsersByRole = totalUsersByRole,
                    TotalCards = totalCards,
                    TotalAssets = totalAssets,
                    TotalSavedCards = totalSavedCards,
                    TotalRevenue = totalRevenue
                };

                res.IsSucess = true;
                res.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                res.Data = dashboard;
            }
            catch (Exception ex)
            {
                res.IsSucess = false;
                res.BusinessCode = BusinessCode.EXCEPTION;
                res.message = ex.Message;
            }
            return res;
        }

        public async Task<ResponseDTO> GetUserByIdAsync(string userId)
        {
            var res = new ResponseDTO();
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    res.IsSucess = false;
                    res.BusinessCode = BusinessCode.NOT_FOUND;
                    res.message = "User not found.";
                    return res;
                }

                var roles = await _userManager.GetRolesAsync(user);

                res.IsSucess = true;
                res.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                res.Data = new UserAdminDTO
                {
                    UserId = user.Id,
                    Email = user.Email ?? "",
                    FullName = $"{user.FirstName} {user.LastName}",
                    Roles = roles.ToList(),
                    BanStatus = user.Ban
                };
            }
            catch (Exception ex)
            {
                res.IsSucess = false;
                res.BusinessCode = BusinessCode.EXCEPTION;
                res.message = ex.Message;
            }
            return res;
        }

        public async Task<ResponseDTO> UnbanUserAsync(string userId)
        {
            var res = new ResponseDTO();
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    res.IsSucess = false;
                    res.BusinessCode = BusinessCode.NOT_FOUND;
                    res.message = "User not found.";
                    return res;
                }

                user.Ban = "InActive";
                await _userManager.UpdateAsync(user);

                res.IsSucess = true;
                res.BusinessCode = BusinessCode.UPDATE_SUCESSFULLY;
                res.message = "User unbanned successfully.";
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
