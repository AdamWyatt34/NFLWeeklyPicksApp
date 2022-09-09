using AutoMapper;
using EmailService;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using NFLWeeklyPicksAPI.Models.Entities;
using System.Text;

namespace NFLWeeklyPicksAPI.Commands.Authentication
{
    public class RegisterUser : IRequest<IdentityResult>
    {
        public UserForRegistrationDto UserForRegistration { get; set; }

        public class Handler : IRequestHandler<RegisterUser, IdentityResult>
        {
            private readonly IMapper _mapper;
            private readonly UserManager<User> _userManager;
            private readonly IEmailSender _emailSender;

            public Handler(IMapper mapper, UserManager<User> userManager, IEmailSender emailSender)
            {
                _mapper = mapper;
                _userManager = userManager;
                _emailSender = emailSender;
            }

            public async Task<IdentityResult> Handle(RegisterUser request, CancellationToken cancellationToken)
            {
                var user = _mapper.Map<User>(request.UserForRegistration);
                var result = await _userManager.CreateAsync(user,
                    request.UserForRegistration.Password);
                if (result.Succeeded)
                {
                    await _userManager.SetTwoFactorEnabledAsync(user, true);
                    await _userManager.AddToRolesAsync(user, request.UserForRegistration.Roles);
                    await SendEmailConfirmationLink(user);
                }

                return result;
            }

            private async Task SendEmailConfirmationLink(User user)
            {
                var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                byte[] tokenGeneratedBytes = Encoding.UTF8.GetBytes(emailToken);
                var token = WebEncoders.Base64UrlEncode(tokenGeneratedBytes);
                var url = $"https://localhost:7066/confirm-email/?token={token}&email={user.Email}";

                var message = new Message(new string[] { user.Email }, "Confirm Email", url, null);

                await _emailSender.SendEmailAsync(message);
            }
        }
    }
}