using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NFLWeeklyPicksAPI.Extensions;
using NFLWeeklyPicksAPI.Models.ESPNDataModels;
using NFLWeeklyPicksAPI.Queries.NFL;
using NFLWeeklyPicksAPI.ViewModels;
using System.Net;

namespace NFLWeeklyPicksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NFLController : ControllerBase
    {
        private readonly IMediator _dispatcher;
        private readonly IHttpClientFactory _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string _apiUrl;

        public NFLController(IMediator dispatcher, IHttpClientFactory httpClient, IConfiguration configuration)
        {
            _dispatcher = dispatcher;
            _httpClient = httpClient;
            _configuration = configuration;
            _apiUrl = _configuration.GetValue<string>("ESPNAPIUrl");
        }

        [Route("weekly-picks"), HttpGet, ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<WeeklyGamesViewModel> GetWeeklyPicks([FromQuery] ListWeeklyGames request) =>
            await _dispatcher.Send(request);

        [Route("weekly-scores"), HttpGet, ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<WeeklyGamesWithScoreViewModel>
            GetWeeklyScores([FromQuery] ListWeeklyGamesWithScores request) => await _dispatcher.Send(request);

        [Route("competition"), HttpGet, ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<CompetitionModel> GetCompetition(int competitonId)
        {
            var apiUrl = $"{_apiUrl}/events/{competitonId}?lang=en&region=us";

            var client = _httpClient.CreateClientWithUrl(apiUrl);

            return await client.GetFromJsonAsync<CompetitionModel>(apiUrl);
        }

        [Route("team"), HttpGet, ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<TeamModel> GetTeam(int season, int teamId)
        {
            var apiUrl = $"{_apiUrl}/seaons/{season}/teams/{teamId}?lang=en&region=us";

            var client = _httpClient.CreateClientWithUrl(apiUrl);

            return await client.GetFromJsonAsync<TeamModel>(apiUrl);
        }
    }
}