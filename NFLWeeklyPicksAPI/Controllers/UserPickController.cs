using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NFLWeeklyPicksAPI.Commands;
using NFLWeeklyPicksAPI.Queries.UserPicks;
using NFLWeeklyPicksAPI.ViewModels;
using System.Net;
using NFLWeeklyPicksAPI.Commands.UserPicks;

namespace NFLWeeklyPicksAPI.Controllers
{
    [Route("api/user-pick")]
    [ApiController]
    [Authorize]
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
        public async Task<List<UserPickWeeklyRecordViewModel>> CalculatePickRecordsWeek(
            [FromRoute] CalculatePickRecordsWeek query) => await _dispatcher.Send(query);

        [Route("unpaid/{Season}/{Week}"), HttpGet, ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IEnumerable<UnpaidPickViewModel>> GetUnpaidPicks([FromRoute] GetUnpaidPicks query) =>
            await _dispatcher.Send(query);

        [Route("mark-unpaid"), HttpPut, ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<Unit> MarkUserPickPaid(MarkUserPicksPaid query) => await _dispatcher.Send(query);
    }
}