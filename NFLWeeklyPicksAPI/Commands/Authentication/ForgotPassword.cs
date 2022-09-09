using EmailService;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Routing;
using NFLWeeklyPicksAPI.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using NFLWeeklyPicksAPI.Options;

namespace NFLWeeklyPicksAPI.Commands.Authentication
{
    public class ForgotPassword : IRequest
    {
        public string EmailAddress { get; set; }

        public class Handler : IRequestHandler<ForgotPassword, Unit>
        {
            private readonly UserManager<User> _userManager;
            private readonly IEmailSender _emailSender;
            private readonly ClientAppOptions _clientOptions;

            public Handler(UserManager<User> userManager, IEmailSender emailSender,
                IOptions<ClientAppOptions> clientOptions)
            {
                _userManager = userManager;
                _emailSender = emailSender;
                _clientOptions = clientOptions.Value;
            }

            public async Task<Unit> Handle(ForgotPassword request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByNameAsync(request.EmailAddress);
                if (user == null)
                    return Unit.Value;

                var emailToken = await _userManager.GeneratePasswordResetTokenAsync(user);

                byte[] tokenGeneratedBytes = Encoding.UTF8.GetBytes(emailToken);
                var token = WebEncoders.Base64UrlEncode(tokenGeneratedBytes);
                var url =
                    $"{_clientOptions.Url}/forgot-password/?token={token}&email={user.Email}";
                //TODO Generate html body
                var message = new Message(new string[] { user.Email }, "Reset Password token", url, null);

                await _emailSender.SendEmailAsync(message);

                return Unit.Value;
            }
        }
    }
}