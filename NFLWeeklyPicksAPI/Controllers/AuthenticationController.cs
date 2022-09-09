using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NFLWeeklyPicksAPI.ActionFilters;
using NFLWeeklyPicksAPI.Behaviors.Interfaces;
using NFLWeeklyPicksAPI.Commands.Authentication;
using NFLWeeklyPicksAPI.Models.Authorization;
using NFLWeeklyPicksAPI.Queries.Authentication;
using System.Formats.Asn1;
using System.Net;
using NFLWeeklyPicksAPI.Models.Entities;

namespace NFLWeeklyPicksAPI.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _dispatcher;
        private readonly UserManager<User> _userManager;

        public AuthenticationController(IMediator dispatcher, UserManager<User> userManager)
        {
            _dispatcher = dispatcher;
            _userManager = userManager;
        }

        //ServiceFilter(typeof(ValidationFilterAttribute)
        [Route(""), HttpPost]
        public async Task<IdentityResult> RegisterUser([FromBody] RegisterUser query) => await _dispatcher.Send(query);

        [Route("login"), HttpPost]
        public async Task<TokenDto> Authenticate([FromBody] ValidateUser query)
        {
            var (result, user) = await _dispatcher.Send(query);
            if (!result)
                return null;

            if (await _userManager.GetTwoFactorEnabledAsync(user))
            {
                await _dispatcher.Send(new GetTwoFactorAuthenticationCode() { Email = user.Email },
                    new CancellationToken());
                return new TokenDto("2FARequired", $"{user.Email}");
            }

            return await _dispatcher.Send(new CreateToken() { User = user, PopulateExpiration = true });
        }

        [Route("forgot-password"), HttpPost, ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task ForgotPassword([FromBody] ForgotPassword query) => await _dispatcher.Send(query);

        [Route("reset-password"), HttpPost, ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<bool> ResetPassword([FromBody] ResetPassword query) => await _dispatcher.Send(query);

        [Route("confirm-email"), HttpPost, ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<bool> ConfirmEmail([FromBody] ConfirmEmail query) => await _dispatcher.Send(query);

        [Route("two-factor"), HttpGet, ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<bool> GetTwoFactorAuthenticationCode([FromQuery] GetTwoFactorAuthenticationCode query) =>
            await _dispatcher.Send(query);

        [Route("verify-two-factor"), HttpPost, ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<TokenDto> TwoFactorVerification([FromBody] TwoFactorVerification request)
        {
            var (result, user) = await _dispatcher.Send(request);

            if (!result)
                return null;

            await _userManager.ResetAccessFailedCountAsync(user);

            return await _dispatcher.Send(new CreateToken() { User = user, PopulateExpiration = true });
        }
    }
}