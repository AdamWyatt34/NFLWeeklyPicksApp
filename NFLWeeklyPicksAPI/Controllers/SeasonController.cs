using MediatR;
using Microsoft.AspNetCore.Mvc;
using NFLWeeklyPicksAPI.Queries.SeasonWeeks;
using NFLWeeklyPicksAPI.ViewModels;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace NFLWeeklyPicksAPI.Controllers
{
    [Route("api/season")]
    [ApiController]
    [Authorize]
    public class SeasonController
    {
        private readonly IMediator _dispatcher;

        public SeasonController(IMediator dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [Route("{Season}/weeks"), HttpGet, ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IEnumerable<SeasonWeeksViewModel>> ListSeasonWeeks([FromRoute] ListSeasonWeeks query) =>
            await _dispatcher.Send(query);

        [Route("{Season}/{Week}"), HttpGet, ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<WeeklyGamesViewModel> ListWeeklyGames([FromRoute] ListWeeklyGames query) =>
            await _dispatcher.Send(query);

        [Route("{Season}/{Week}/score"), HttpGet, ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<WeeklyGamesWithScoreViewModel> ListWeeklyGamesWithScore(
            [FromRoute] ListWeeklyGamesWithScore query) => await _dispatcher.Send(query);

        [Route(""), HttpGet, ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IEnumerable<SeasonViewModel>> ListSeasons([FromRoute] ListSeasons query) =>
            await _dispatcher.Send(query);

        [Route("{SeasonId}/week"), HttpGet, ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IEnumerable<WeekViewModel>> ListWeeks([FromRoute] ListWeeks query) =>
            await _dispatcher.Send(query);
    }
}