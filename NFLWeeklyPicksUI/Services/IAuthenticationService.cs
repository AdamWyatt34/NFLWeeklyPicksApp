using System.Net;
using Microsoft.AspNetCore.Identity;
using NFLWeeklyPicksUI.Models.Authentication;

namespace NFLWeeklyPicksUI.Services
{
    public interface IAuthenticationService
    {
        Task<RegisterResponseViewModel> RegisterUser(RegisterUserViewModel registerUser);

        Task<bool> ConfirmEmail(string token, string email);

        Task<LoginToken> Login(string username, string password);

        Task Logout();
        Task<string> RefreshToken();

        Task<HttpStatusCode> ForgotPassword(string email);

        Task<bool> ResetPassword(ResetPasswordModel resetPasswordModel);
        Task<LoginToken> TwoFactorVerification(string email, string provider, string twoFactorToken);
    }
}