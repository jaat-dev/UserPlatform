using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using UserPlatform.Domain.Entities;
using UserPlatform.Domain.Models;

namespace UserPlatform.API.Helpers.Interfaces
{
    public interface IUserHelper
    {
        Task<User> GetUserAsync(Guid id);

        Task<User> GetUserAsync(string email);

        Task<IdentityResult> AddUserAsync(User user, string password);

        Task AddUserToRoleAsync(User user, string roleName);

        Task CheckRoleAsync(string roleName);

        Task<IdentityResult> UpdateUserAsync(User user);

        Task<SignInResult> LoginAsync(LoginViewModel model);

        Task LogoutAsync();
    }
}
