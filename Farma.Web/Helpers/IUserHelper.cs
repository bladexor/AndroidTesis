

namespace Farma.Web.Helpers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Data.Entities;
    using Farma.Web.Models;
    using Microsoft.AspNetCore.Identity;

    public interface IUserHelper
    {
        Task<User> GetUserByEmailAsync(string email);

        Task<User> GetUserWithCitiesandDonations(string email);

        Task<bool> ExistUserAsync(string email);

        Task<IdentityResult> AddUserAsync(User user, string password);

        Task<SignInResult> LoginAsync(LoginViewModel model);

        Task LogoutAsync();

        Task<IdentityResult> UpdateUserAsync(User user);

        Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword);
        
        Task<SignInResult> ValidatePasswordAsync(User user, string password);

        Task CheckRoleAsync(string roleName);

        Task AddUserToRoleAsync(User user, string roleName);

        Task<bool> IsUserInRoleAsync(User user, string roleName);


        //Para Confirmacion de Email Al Registrarse un Usuario-------------------------
        Task<string> GenerateEmailConfirmationTokenAsync(User user);

        Task<IdentityResult> ConfirmEmailAsync(User user, string token);

        Task<User> GetUserByIdAsync(string userId);
        //--------------------------------------------------------------------------------

        //Para Olvido de Contraseña y reseteo de la misma
        Task<string> GeneratePasswordResetTokenAsync(User user);

        Task<IdentityResult> ResetPasswordAsync(User user, string token, string password);


        Task<List<User>> GetAllUsersAsync();

        Task RemoveUserFromRoleAsync(User user, string roleName);

        Task DeleteUserAsync(User user);

        Task<User> GetUserwithDonationsAsync(string userEmail);
    }

}
