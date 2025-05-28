using Microsoft.AspNetCore.Identity;
using Monopolizers.Repository.DB;
using Monopolizers.Repository.Models;

namespace Monopolizers.Repository.Repositories
{
    public interface IAccountRepository
    {
        Task<string> SignInAsync(SignInModel model);
        Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);
        Task<IdentityResult> AddUserToRoleAsync(ApplicationUser user, string roleName);
        Task<ApplicationUser?> FindByIdAsync(string userId);
        Task<IdentityResult> UpdateAsync(ApplicationUser user);
        Task<ApplicationUser?> FindByUsernameAsync(string username);
        Task<bool> BanUserAsync(string userId);

    }
}
