

namespace Farma.Web.Helpers
{
    using System.Threading.Tasks;
    using Data.Entities;
    using Farma.Web.Models;
    using Microsoft.AspNetCore.Identity;

    public interface IUserHelper
    {
        Task<User> GetUserByEmailAsync(string email);

        Task<IdentityResult> AddUserAsync(User user, string password);

        Task<SignInResult> LoginAsync(LoginViewModel model);

        Task LogoutAsync();

    }

}
