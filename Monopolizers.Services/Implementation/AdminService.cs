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
                var users = _userManager.Users.AsQueryable();
                var planPurchases = _unitOfWork.PlanPurchaseRepository
                    .GetQueryable()
                    .Include(p => p.Plan);
                var walletTransactions = _unitOfWork.WalletTransactionRepository
                    .GetQueryable()
                    .Include(t => t.Wallet)
                    .ThenInclude(w => w.User);

                async Task<List<DashboardStatsDTO>> BuildStats(string period)
                {
                    Func<DateTime?, DateTime> keySelector = period switch
                    {
                        "daily" => d => d?.Date ?? DateTime.MinValue,
                        "monthly" => d => d.HasValue ? new DateTime(d.Value.Year, d.Value.Month, 1) : DateTime.MinValue,
                        "yearly" => d => d.HasValue ? new DateTime(d.Value.Year, 1, 1) : DateTime.MinValue,
                        _ => throw new ArgumentException("Invalid period")
                    };

                    var allPeriods = new HashSet<DateTime>();

                    var userPeriods = users.AsEnumerable()
                        .Select(u => keySelector(u.CreatedAt))
                        .Where(d => d != DateTime.MinValue)
                        .Distinct()
                        .ToList();

                    var planPeriods = planPurchases.AsEnumerable()
                        .Select(p => keySelector(p.PurchasedAt))
                        .Where(d => d != DateTime.MinValue)
                        .Distinct()
                        .ToList();

                    var transPeriods = walletTransactions.AsEnumerable()
                        .Select(t => keySelector(t.CreatedAt))
                        .Where(d => d != DateTime.MinValue)
                        .Distinct()
                        .ToList();

                    foreach (var p in userPeriods.Concat(planPeriods).Concat(transPeriods))
                        allPeriods.Add(p);

                    var stats = new List<DashboardStatsDTO>();

                    foreach (var periodDate in allPeriods.OrderBy(d => d))
                    {
                        var newUsers = users.AsEnumerable()
                            .Count(u => keySelector(u.CreatedAt) == periodDate);

                        var activeFromTransactions = walletTransactions.AsEnumerable()
                            .Where(t => keySelector(t.CreatedAt) == periodDate)
                            .Select(t => t.Wallet.User.Id)
                            .Distinct()
                            .ToList();

                        var activeFromPlans = planPurchases.AsEnumerable()
                            .Where(p => keySelector(p.PurchasedAt) == periodDate)
                            .Select(p => p.UserId)
                            .Distinct()
                            .ToList();

                        var activeUsers = activeFromTransactions
                            .Union(activeFromPlans)
                            .Distinct()
                            .Count();

                        var planCount = planPurchases.AsEnumerable()
                            .Count(p => keySelector(p.PurchasedAt) == periodDate);

                        var plans = planPurchases.AsEnumerable()
                            .Where(p => keySelector(p.PurchasedAt) == periodDate)
                            .GroupBy(p => p.Plan.Name)
                            .Select(g => new { g.Key, Count = g.Count() })
                            .ToList();

                        var totalTopUp = walletTransactions.AsEnumerable()
                            .Where(t => keySelector(t.CreatedAt) == periodDate &&
                                        (t.Type == TransactionTypes.Deposit || t.Type == TransactionTypes.TopUp))
                            .Sum(t => (decimal?)t.Amount) ?? 0;

                        var totalPlanRevenue = planPurchases.AsEnumerable()
                            .Where(p => keySelector(p.PurchasedAt) == periodDate)
                            .Sum(p => (decimal?)p.Price) ?? 0;

                        var totalRevenue = totalTopUp + totalPlanRevenue;

                        var stat = new DashboardStatsDTO
                        {
                            Period = periodDate,
                            NewUsers = newUsers,
                            ActiveUsers = activeUsers,
                            PlanPurchases = planCount,
                            TotalTopUp = totalTopUp,
                            TotalPlanRevenue = totalPlanRevenue,
                            TotalRevenue = totalRevenue
                        };

                        foreach (var p in plans)
                            stat.PlansBought[p.Key] = p.Count;

                        stats.Add(stat);
                    }

                    return stats;
                }

                var dashboard = new DashboardOverviewDTO
                {
                    DailyStats = await Task.Run(() => BuildStats("daily")),
                    MonthlyStats = await Task.Run(() => BuildStats("monthly")),
                    YearlyStats = await Task.Run(() => BuildStats("yearly"))
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
