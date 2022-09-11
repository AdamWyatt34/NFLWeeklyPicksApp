using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NFLWeeklyPicksAPI.Commands;
using NFLWeeklyPicksAPI.Queries.UserPicks;
using NFLWeeklyPicksAPI.ViewModels;
using System.Net;

namespace NFLWeeklyPicksAPI.Controllers
{
    [Route("api/user-pick")]
    [ApiController]
    // TODO Uncomment [Authorize]
    public class UserPickController : ControllerBase
    {
        private readonly IMediator _dispatcher;

        public UserPickController(IMediator dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [Route(""), HttpPost, ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<int> AddUserPick([FromBody] AddUserPick query) => await _dispatcher.Send(query);

        [Route("{UserPickId}"), HttpGet, ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<UserPicks> GetUserPick([FromRoute] GetUserPick query) => await _dispatcher.Send(query);

        [Route(""), HttpPut, ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<Unit> UpdateUserPick(UpdateUserPick query) => await _dispatcher.Send(query);

        [Route("{Season}/{WeekNumber}"), HttpGet, ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<List<WeeklyGameWithScoreAndWinnerViewModel>> CalculatePickRecordsWeek(
            [FromRoute] CalculatePickRecordsWeek query) => await _dispatcher.Send(query);
    }
}