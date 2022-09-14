using MediatR;
using Microsoft.EntityFrameworkCore;
using NFLWeeklyPicksAPI.Queries.NFL;

namespace NFLWeeklyPicksAPI.Commands.Competitions;

public class SyncCompetitionsWithEspn : IRequest<Unit>
{
    public class Handler : IRequestHandler<SyncCompetitionsWithEspn, Unit>
    {
        private readonly IMediator _dispatcher;
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;
        private string _apiUrl;

        public Handler(IMediator dispatcher, ApplicationDbContext db, IConfiguration configuration)
        {
            _dispatcher = dispatcher;
            _db = db;
            _configuration = configuration;
            _apiUrl = _configuration.GetValue<string>("ESPNAPIUrl");
        }

        public async Task<Unit> Handle(SyncCompetitionsWithEspn request, CancellationToken cancellationToken)
        {
            //Get last season week
            var lastSeasonWeek =
                await _db.SeasonWeeks
                    .Include(sw => sw.Season)
                    .OrderByDescending(sw => sw.EndDate)
                    .FirstAsync(cancellationToken);

            var competitions = await _dispatcher.Send(new ListWeeklyGames()
            {
                Season = lastSeasonWeek.Season.Year,
                Week = lastSeasonWeek.WeekNumber
            }, cancellationToken);

            //Add each competition to competition table
            foreach (var competition in competitions.Games)
            {
                await _db.Competitions.AddAsync(new Models.Entities.Competitions()
                {
                    AwayTeamId = competition.AwayTeam.Id,
                    AwayTeamScoreUrl =
                        $"{_apiUrl}/events/{competition.CompetitionId}/competitions/{competition.CompetitionId}/competitors/{competition.AwayTeam.Id}/score",
                    EspnCompetitionId = competition.CompetitionId,
                    GameDate = competition.GameDate,
                    GameName = competition.GameName,
                    HomeTeamId = competition.HomeTeam.Id,
                    HomeTeamScoreUrl =
                        $"{_apiUrl}/events/{competition.CompetitionId}/competitions/{competition.CompetitionId}/competitors/{competition.HomeTeam.Id}/score",
                    Odds = competition.Odds,
                    SeasonWeeks = lastSeasonWeek
                }, cancellationToken);
            }

            return Unit.Value;
        }
    }
}