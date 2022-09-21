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
                    .Where(sw => sw.Season.Year == 2022 && sw.WeekNumber == 1)
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
                var homeTeamId = await _db.Teams
                    .Where(t => t.Abbreviation == competition.HomeTeam.Abbreviation)
                    .Select(t => t.TeamsId)
                    .FirstAsync(cancellationToken);

                var awayTeamId = await _db.Teams
                    .Where(t => t.Abbreviation == competition.AwayTeam.Abbreviation)
                    .Select(t => t.TeamsId)
                    .FirstAsync(cancellationToken);

                await _db.Competitions.AddAsync(new Models.Entities.Competitions()
                {
                    AwayTeamId = awayTeamId,
                    AwayTeamScoreUrl =
                        $"{_apiUrl}/events/{competition.EspnCompetitonId}/competitions/{competition.EspnCompetitonId}/competitors/{competition.AwayTeam.Id}/score",
                    EspnCompetitionId = competition.EspnCompetitonId,
                    GameDate = competition.GameDate,
                    GameName = competition.GameName,
                    HomeTeamId = homeTeamId,
                    HomeTeamScoreUrl =
                        $"{_apiUrl}/events/{competition.EspnCompetitonId}/competitions/{competition.EspnCompetitonId}/competitors/{competition.HomeTeam.Id}/score",
                    Odds = competition.Odds,
                    SeasonWeeksId = lastSeasonWeek.SeasonWeeksId,
                }, cancellationToken);
            }

            await _db.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}