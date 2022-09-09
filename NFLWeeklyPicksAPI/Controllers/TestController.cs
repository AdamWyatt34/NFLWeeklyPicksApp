using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NFLWeeklyPicksAPI.Models.ESPNDataModels;

namespace NFLWeeklyPicksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly ApplicationDbContext _db;

        public TestController(IHttpClientFactory httpClient, ApplicationDbContext db)
        {
            _httpClient = httpClient;
            _db = db;
        }

        [Route(""), HttpGet]
        public async Task<ScoreModel> TestMethod()
        {
            //Weekly Game Model
            //var testUrl = "https://sports.core.api.espn.com/v2/sports/football/leagues/nfl/seasons/2022/types/2/weeks/1/events";
            //Competition Model
            //var testUrl = "http://sports.core.api.espn.com/v2/sports/football/leagues/nfl/events/401437654?lang=en&region=us"; 
            //Team Model
            //var testUrl = "http://sports.core.api.espn.com/v2/sports/football/leagues/nfl/seasons/2022/teams/14?lang=en&region=us";
            //score Model
            var testUrl = "http://sports.core.api.espn.com/v2/sports/football/leagues/nfl/events/401437654/competitions/401437654/competitors/14/score?lang=en&region=us";
            var client = _httpClient.CreateClient();
            client.BaseAddress = new Uri(testUrl);

            var test = _db.UserPicks.Include(up => up.PickLineItems);

            //var output = await client.GetFromJsonAsync<WeeklyGameModel>(testUrl);
            var output = await client.GetFromJsonAsync<ScoreModel>(testUrl);
            return output;
        }
    }
}
