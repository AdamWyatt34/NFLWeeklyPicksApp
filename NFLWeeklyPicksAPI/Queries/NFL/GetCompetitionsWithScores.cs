using MediatR;
using Microsoft.EntityFrameworkCore;
using NFLWeeklyPicksAPI.Extensions;
using NFLWeeklyPicksAPI.Models.ESPNDataModels;
using NFLWeeklyPicksAPI.ViewModels;

namespace NFLWeeklyPicksAPI.Queries.NFL;

public class GetCompetitionsWithScores : IRequest<WeeklyGameWithScoreViewModel>
{
    public int Season { get; set; }
    public int WeekNumber { get; set; }
    public RefLink CompetitionUrls { get; set; }

    public class Handler : IRequestHandler<GetCompetitionsWithScores, WeeklyGameWithScoreViewModel>
    {
        private readonly IMediator _dispatcher;
        private readonly IHttpClientFactory _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _db;
        private readonly string _apiUrl;

        public Handler(IMediator dispatcher, IHttpClientFactory httpClient, IConfiguration configuration,
            ApplicationDbContext db)
        {
            _dispatcher = dispatcher;
            _httpClient = httpClient;
            _configuration = configuration;
            _db = db;
            _apiUrl = _configuration.GetValue<string>("ESPNAPIUrl");
        }

        public async Task<WeeklyGameWithScoreViewModel> Handle(GetCompetitionsWithScores request,
            CancellationToken cancellationToken)
        {
            var client = _httpClient.CreateClientWithUrl(request.CompetitionUrls.Ref);
            var competition =
                await client.GetFromJsonAsync<CompetitionModel>(request.CompetitionUrls.Ref, cancellationToken);
            var competitionId = int.Parse(competition.id);
            var homeTeamId = int.Parse(competition.competitions.First().competitors
                .First(c => c.homeAway == "home").id);
            var awayTeamId = int.Parse(competition.competitions.First().competitors
                .First(c => c.homeAway == "away").id);

            var teams = await _dispatcher.Send(new GetTeamsWithScore()
            {
                CompetitionId = competitionId,
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

            var userPicks = await _db.UserPicks
                .Include(up => up.PickLineItems)
                .Where(up => up.Season == request.Season)
                .Where(up => up.Week == request.WeekNumber)
                .Where(up => up.PickLineItems.Any(up => up.CompetitionId == competitionId))
                .ToListAsync(cancellationToken);

            var userIds = userPicks.Select(up => up.UserId.ToString())
                .Distinct()
                .ToList();

            var users = await _db.Users
                .Where(u => userIds.Contains(u.Id))
                .Select(u => new { u.Id, u.UserName })
                .ToListAsync(cancellationToken);

            var picks = new List<UserPickScoreViewModel>();

            foreach (var pick in userPicks)
            {
                var selectedTeamId = pick.PickLineItems.First(pl => pl.CompetitionId == competitionId).PickTeamId;
                var team = teams.Find(t => t.Id == selectedTeamId);
                picks.Add(new UserPickScoreViewModel()
                {
                    SelectedTeam = team.FullName,
                    SelectedTeamAbbreviation = team.Abbreviation,
                    Username = users.Find(u => u.Id == pick.UserId.ToString()).UserName
                });
            }

            return new WeeklyGameWithScoreViewModel()
            {
                CompetitionId = long.Parse(competition.id),
                GameName = competition.name,
                GameDate = new DateTime(tempDate.Year, tempDate.Month, tempDate.Day, tempDate.Hour, tempDate.Minute,
                    tempDate.Second),
                HomeTeam = teams.First(t => t.Id == homeTeamId),
                AwayTeam = teams.First(t => t.Id == awayTeamId),
                Odds = odds,
                UserPicks = picks
            };
        }
    }
}