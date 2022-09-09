using MediatR;
using Microsoft.AspNetCore.Mvc;
using NFLWeeklyPicksAPI.Commands.Authentication;
using NFLWeeklyPicksAPI.Models.Authorization;

namespace NFLWeeklyPicksAPI.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IMediator _dispatcher;

        public TokenController(IMediator dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [Route("refresh"), HttpPost]
        public async Task<TokenDto> RefreshToken([FromBody] RefreshToken query) => await _dispatcher.Send(query);
    }
}