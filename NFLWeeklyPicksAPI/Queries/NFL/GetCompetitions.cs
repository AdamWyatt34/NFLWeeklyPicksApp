using MediatR;
using NFLWeeklyPicksAPI.Extensions;
using NFLWeeklyPicksAPI.Models.ESPNDataModels;
using NFLWeeklyPicksAPI.ViewModels;

namespace NFLWeeklyPicksAPI.Queries.NFL
{
    public class GetCompetitions : IRequest<WeeklyGameViewModel>
    {
        public int Season { get; set; }
        public RefLink CompetitionUrls { get; set; }

        internal class Handler : IRequestHandler<GetCompetitions, WeeklyGameViewModel>
        {
            private readonly IMediator _dispatcher;
            private readonly IHttpClientFactory _httpClient;
            private readonly IConfiguration _configuration;
            private readonly string _apiUrl;

            public Handler(IMediator dispatcher, IHttpClientFactory httpClient, IConfiguration configuration)
            {
                _dispatcher = dispatcher;
                _httpClient = httpClient;
                _configuration = configuration;
                _apiUrl = _configuration.GetValue<string>("ESPNAPIUrl");
            }

            public async Task<WeeklyGameViewModel> Handle(GetCompetitions request, CancellationToken cancellationToken)
            {
                var client = _httpClient.CreateClientWithUrl(request.CompetitionUrls.Ref);
                var competition =
                    await client.GetFromJsonAsync<CompetitionModel>(request.CompetitionUrls.Ref, cancellationToken);
                var competitionId = int.Parse(competition.id);
                var homeTeamId = int.Parse(competition.competitions.First().competitors.Where(c => c.homeAway == "home")
                    .First().id);
                var awayTeamId = int.Parse(competition.competitions.First().competitors.Where(c => c.homeAway == "away")
                    .First().id);

                var teams = await _dispatcher.Send(new GetTeams()
                {
                    Season = request.Season,
                    TeamIds = new List<int>()
                    {
                        homeTeamId,
                        awayTeamId
                    }
                }, cancellationToken);

                var odds = await _dispatcher.Send(new GetCompetitionsOdds() { CompetitoinId = competitionId },
                    cancellationToken);

                var tempDate = DateTime.Parse(competition.date);

                return new WeeklyGameViewModel
                {
                    EspnCompetitonId = long.Parse(competition.id),
                    GameName = competition.name,
                    GameDate = new DateTime(tempDate.Year, tempDate.Month, tempDate.Day, tempDate.Hour, tempDate.Minute,
                        tempDate.Second),
                    HomeTeam = teams.Where(t => t.Id == homeTeamId).First(),
                    AwayTeam = teams.Where(t => t.Id == awayTeamId).First(),
                    Odds = odds
                };
            }
        }
    }
}