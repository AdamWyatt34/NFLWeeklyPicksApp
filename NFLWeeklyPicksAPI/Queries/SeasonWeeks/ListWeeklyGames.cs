using MediatR;
using Microsoft.EntityFrameworkCore;
using NFLWeeklyPicksAPI.ViewModels;

namespace NFLWeeklyPicksAPI.Queries.SeasonWeeks;

public class ListWeeklyGames : IRequest<WeeklyGamesViewModel>
{
    public int Season { get; set; }
    public int Week { get; set; }

    public class Handler : IRequestHandler<ListWeeklyGames, WeeklyGamesViewModel>
    {
        private readonly ApplicationDbContext _db;

        public Handler(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<WeeklyGamesViewModel> Handle(ListWeeklyGames request, CancellationToken cancellationToken)
        {
            var competitions = await _db.Competitions
                .Where(c => c.SeasonWeeks.Season.Year == request.Season)
                .Where(c => c.SeasonWeeks.WeekNumber == request.Week)
                .Select(WeeklyGameViewModel.Selector)
                .ToListAsync(cancellationToken);

            await SupplementViewModels(competitions, request.Season, cancellationToken);

            return new WeeklyGamesViewModel()
            {
                Season = request.Season,
                Week = request.Week,
                Games = competitions
            };
        }

        private async Task SupplementViewModels(List<WeeklyGameViewModel> viewModels, int year,
            CancellationToken cancellationToken)
        {
            foreach (var viewModel in viewModels)
            {
                //Populate Home Team
                var homeTeam = await _db.Teams
                    .Where(t => t.TeamsId == viewModel.HomeTeam.Id)
                    .FirstAsync(cancellationToken);

                viewModel.HomeTeam.Abbreviation = homeTeam.Abbreviation;
                viewModel.HomeTeam.Location = homeTeam.Location;
                viewModel.HomeTeam.Nickname = homeTeam.Nickname;
                viewModel.HomeTeam.FullName = homeTeam.FullName;
                viewModel.HomeTeam.LogoURL = homeTeam.LogoURL;

                viewModel.HomeTeam.Record = await _db.TeamSeasonRecords
                    .Where(tsr => tsr.Season.Year == year)
                    .Where(tsr => tsr.TeamId == viewModel.HomeTeam.Id)
                    .Select(tsr => tsr.Record)
                    .FirstAsync(cancellationToken);

                //Populate Away Team
                var awayTeam = await _db.Teams
                    .Where(t => t.TeamsId == viewModel.AwayTeam.Id)
                    .FirstAsync(cancellationToken);

                viewModel.AwayTeam.Abbreviation = awayTeam.Abbreviation;
                viewModel.AwayTeam.Location = awayTeam.Location;
                viewModel.AwayTeam.Nickname = awayTeam.Nickname;
                viewModel.AwayTeam.FullName = awayTeam.FullName;
                viewModel.AwayTeam.LogoURL = awayTeam.LogoURL;

                viewModel.AwayTeam.Record = await _db.TeamSeasonRecords
                    .Where(tsr => tsr.Season.Year == year)
                    .Where(tsr => tsr.TeamId == viewModel.AwayTeam.Id)
                    .Select(tsr => tsr.Record)
                    .FirstAsync(cancellationToken);
            }
        }
    }
}