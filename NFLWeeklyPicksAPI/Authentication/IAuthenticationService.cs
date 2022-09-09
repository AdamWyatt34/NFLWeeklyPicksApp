using Microsoft.AspNetCore.Identity;
using NFLWeeklyPicksAPI.Models.Entities;

namespace NFLWeeklyPicksAPI.Authentication
{
    public interface IAuthenticationService
    {
        Task<IdentityResult> RegisterUser(UserForRegistrationDto user);
    }
}