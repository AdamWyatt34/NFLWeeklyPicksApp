using MediatR;
using Microsoft.AspNetCore.Identity;
using NFLWeeklyPicksAPI.Models.Authorization;
using NFLWeeklyPicksAPI.Models.Entities;

namespace NFLWeeklyPicksAPI.Commands.Authentication;

public class TwoFactorVerification : IRequest<(bool, User)>
{
    public string Email { get; set; }
    public string Provider { get; set; }
    public string TwoFactorToken { get; set; }

    public class Handler : IRequestHandler<TwoFactorVerification, (bool, User)>
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<TwoFactorVerification> _logger;

        public Handler(UserManager<User> userManager, ILogger<TwoFactorVerification> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<(bool, User)> Handle(TwoFactorVerification request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
                return (false, null);

            var validVerification =
                await _userManager.VerifyTwoFactorTokenAsync(user, request.Provider, request.TwoFactorToken);

            if (!validVerification)
                _logger.LogWarning($"{nameof(ValidateUser)}: Authentication failed. Wrong 2FA Code.");


            return (validVerification, user);
        }
    }
}