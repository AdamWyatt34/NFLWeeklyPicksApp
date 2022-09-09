using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using NFLWeeklyPicksAPI.Models.Entities;
using System.Text;

namespace NFLWeeklyPicksAPI.Commands.Authentication
{
    public class ConfirmEmail : IRequest<bool>
    {
        public string Token { get; set; }
        public string Email { get; set; }

        public class Handler : IRequestHandler<ConfirmEmail, bool>
        {
            private readonly UserManager<User> _userManager;

            public Handler(UserManager<User> userManager)
            {
                _userManager = userManager;
            }

            public async Task<bool> Handle(ConfirmEmail request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);

                if (user == null)
                    return false;

                var decodeBytes = WebEncoders.Base64UrlDecode(request.Token);
                var tokenDecoded = Encoding.UTF8.GetString(decodeBytes);
                var result = await _userManager.ConfirmEmailAsync(user, tokenDecoded);

                return result.Succeeded;
            }
        }
    }
}