using Microsoft.AspNetCore.Identity;
using NFLWeeklyPicksAPI.Models.Entities;

namespace NFLWeeklyPicksAPI.Extensions;

public static class HttpContextExtensions
{
    public static async Task<User> GetCurrentUser(this HttpContext context, UserManager<User> userManager)
    {
        var currentUserFromContext = context.User;
        var currentUserName = currentUserFromContext.Identity.Name;
        return await userManager.FindByNameAsync(currentUserName);
    }
}