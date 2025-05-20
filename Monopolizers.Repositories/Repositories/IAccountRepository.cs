using Microsoft.AspNetCore.Identity;
using Monopolizers.Repository.Models;

namespace Monopolizers.Repository.Repositories
{
    public interface IAccountRepository
    {
        public Task<IdentityResult> SignUpAsync(SignUpModel model);
        public Task<string> SignInAsync(SignInModel model);
    }
}
