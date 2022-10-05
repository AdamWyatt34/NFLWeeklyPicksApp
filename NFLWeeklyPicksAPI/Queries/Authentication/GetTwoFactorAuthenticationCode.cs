using System.Text;
using EmailService;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NFLWeeklyPicksAPI.Models.Entities;

namespace NFLWeeklyPicksAPI.Queries.Authentication
{
    public class GetTwoFactorAuthenticationCode : IRequest<bool>
    {
        [FromQuery] public string Email { get; set; }

        public class Handler : IRequestHandler<GetTwoFactorAuthenticationCode, bool>
        {
            private readonly UserManager<User> _userManager;
            private readonly IEmailSender _emailSender;

            public Handler(UserManager<User> userManager, IEmailSender emailSender)
            {
                _userManager = userManager;
                _emailSender = emailSender;
            }

            public async Task<bool> Handle(GetTwoFactorAuthenticationCode request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);

                if (user == null)
                    return false;

                var providers = await _userManager.GetValidTwoFactorProvidersAsync(user);
                if (!providers.Contains("Email"))
                    return false;

                var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");

                var message = new Message(new string[] { user.Email }, "Authentication Code", token, null);
                await _emailSender.SendEmailAsync(message);

                return true;
            }

            private string GenerateBody(string token)
            {
                var sb = new StringBuilder();
                sb.Append("Please enter this token in the app to log in <br/><br/>");
                sb.Append(token);
                return sb.ToString();
            }
        }
    }
}