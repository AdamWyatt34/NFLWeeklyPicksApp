using EmailService;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using NFLWeeklyPicksAPI.Models.Entities;
using NFLWeeklyPicksAPI.Options;
using NFLWeeklyPicksAPI.Queries.Authentication;

namespace NFLWeeklyPicksAPI.Commands.Authentication
{
    public class ValidateUser : IRequest<(bool, User)>
    {
        public string UserName { get; set; }

        public string Password { get; set; }
        //public UserForAuthenticationDto User { get; set; }
        //public string TwoFactorCode { get; set; }

        public class Handler : IRequestHandler<ValidateUser, (bool, User)>
        {
            private readonly UserManager<User> _userManager;
            private readonly IEmailSender _emailSender;
            private readonly ILogger<ValidateUser> _logger;
            private readonly SignInManager<User> _signInManager;
            private readonly IMediator _dispatcher;
            private readonly ClientAppOptions _clientOptions;

            public Handler(UserManager<User> userManager, IEmailSender emailSender, ILogger<ValidateUser> logger,
                SignInManager<User> signInManager, IMediator dispatcher, IOptions<ClientAppOptions> clientOptions)
            {
                _userManager = userManager;
                _emailSender = emailSender;
                _logger = logger;
                _signInManager = signInManager;
                _dispatcher = dispatcher;
                _clientOptions = clientOptions.Value;
            }

            public async Task<(bool, User)> Handle(ValidateUser request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByNameAsync(request.UserName);

                if (user == null)
                    return (false, null);

                var result = await _signInManager.PasswordSignInAsync(user, request.Password, true, true);

                if (!result.Succeeded && !result.RequiresTwoFactor)
                    _logger.LogWarning($"{nameof(ValidateUser)}: Authentication failed. Wrong user name or password.");

                if (result.IsLockedOut)
                    await HandleLockout(user.Email);

                return (result.Succeeded || result.RequiresTwoFactor, user);
            }

            private async Task HandleLockout(string email)
            {
                var user = await _userManager.FindByEmailAsync(email);

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var url = $"{_clientOptions.Url}/reset-password/{token}";

                var content = $@"Your account is locked out due to too many unsuccessful sign-in attempts
                                to reset your password click this link {url}";

                var message = new Message(new[] { user.Email }, "Locked out account information", content, null);
                await _emailSender.SendEmailAsync(message);
            }
        }
    }
}