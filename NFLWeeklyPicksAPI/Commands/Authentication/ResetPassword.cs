using System.Text;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using NFLWeeklyPicksAPI.Models.Entities;

namespace NFLWeeklyPicksAPI.Commands.Authentication
{
    public class ResetPassword : IRequest<bool>
    {
        public string Password { get; set; }
        public string ConfirmationPassword { get; set; } //TODO Add Validator for this
        public string Email { get; set; }
        public string Token { get; set; }

        public class Handler : IRequestHandler<ResetPassword, bool>
        {
            private readonly UserManager<User> _userManager;

            public Handler(UserManager<User> userManager)
            {
                _userManager = userManager;
            }

            public async Task<bool> Handle(ResetPassword request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);

                if (user == null)
                    return false;

                var decodeBytes = WebEncoders.Base64UrlDecode(request.Token);
                var tokenDecoded = Encoding.UTF8.GetString(decodeBytes);
                var resultPassResult = await _userManager.ResetPasswordAsync(user, tokenDecoded, request.Password);

                if (resultPassResult.Succeeded)
                    if (await _userManager.IsLockedOutAsync(user))
                    {
                        await _userManager.SetLockoutEndDateAsync(user, new DateTimeOffset(new
                            DateTime(1000, 1, 1, 1, 1, 1)));
                    }

                return resultPassResult.Succeeded;
            }
        }
    }
}