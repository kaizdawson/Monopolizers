using Microsoft.AspNetCore.Identity;
using Monopolizers.Repository.Models;
using Monopolizers.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopolizers.Service.Services
{
    public interface IAccountService
    {
        Task<IdentityResult> SignUpAsync(SignUpModel model);
        Task<string> SignInAsync(SignInModel model);
    }
}
