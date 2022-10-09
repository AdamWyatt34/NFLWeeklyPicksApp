using MediatR;
using Microsoft.EntityFrameworkCore;
using NFLWeeklyPicksAPI.Models.Entities;
using NFLWeeklyPicksAPI.Queries.NFL;
using NFLWeeklyPicksAPI.ViewModels;

namespace NFLWeeklyPicksAPI.Queries.SeasonWeeks;

public class ListWeeklyGamesWithScore : IRequest<WeeklyGamesWithScoreViewModel>
{
    public int Season { get; set; }
    public int Week { get; set; }

    public class Handler : IRequestHandler<ListWeeklyGamesWithScore, WeeklyGamesWithScoreViewModel>
    {
        private readonly ApplicationDbContext _db;
        private readonly IMediator _dispatcher;

        public Handler(ApplicationDbContext db, IMediator dispatcher)
        {
            _db = db;
            _dispatcher = dispatcher;
        }

        public async Task<WeeklyGamesWithScoreViewModel> Handle(ListWeeklyGamesWithScore request,
            CancellationToken cancellationToken)
        {
            var competitions = await _db.Competitions
                .Where(c => c.SeasonWeeks.Season.Year == request.Season)
                .Where(c => c.SeasonWeeks.WeekNumber == request.Week)
                .Select(WeeklyGameWithScoreViewModel.Selector)
                .ToListAsync(cancellationToken);

            await SupplementViewModels(competitions, request.Season, cancellationToken);

            return new WeeklyGamesWithScoreViewModel()
            {
                Season = request.Season,
                Week = request.Week,
                Games = competitions
            };
        }

        private async Task SupplementViewModels(List<WeeklyGameWithScoreViewModel> viewModels, int year,
            CancellationToken cancellationToken)
        {
            foreach (var viewModel in viewModels)
            {
                //Populate Home Team with Score
                var homeTeam = await _db.Teams
                    .Where(t => t.TeamsId == viewModel.HomeTeam.Id)
                    .FirstAsync(cancellationToken);

                viewModel.HomeTeam.Abbreviation = homeTeam.Abbreviation;
                viewModel.HomeTeam.Location = homeTeam.Location;
                viewModel.HomeTeam.Nickname = homeTeam.Nickname;
                viewModel.HomeTeam.FullName = homeTeam.FullName;
                viewModel.HomeTeam.LogoURL = homeTeam.LogoURL;
                //Score for home team
                viewModel.HomeTeam.Score = await _dispatcher.Send(new GetTeamScore()
                {
                    CompetitionId = (int)viewModel.EspnCompetitonId,
                    TeamId = homeTeam.EspnTeamId
                }, cancellationToken);

                viewModel.HomeTeam.Record = await _db.TeamSeasonRecords
                    .Where(tsr => tsr.Season.Year == year)
                    .Where(tsr => tsr.TeamId == viewModel.HomeTeam.Id)
                    .Select(tsr => tsr.Record)
                    .FirstAsync(cancellationToken);

                //Populate Away Team with Score
                var awayTeam = await _db.Teams
                    .Where(t => t.TeamsId == viewModel.AwayTeam.Id)
                    .FirstAsync(cancellationToken);

                viewModel.AwayTeam.Abbreviation = awayTeam.Abbreviation;
                viewModel.AwayTeam.Location = awayTeam.Location;
                viewModel.AwayTeam.Nickname = awayTeam.Nickname;
                viewModel.AwayTeam.FullName = awayTeam.FullName;
                viewModel.AwayTeam.LogoURL = awayTeam.LogoURL;
                //Score for home team
                viewModel.AwayTeam.Score = await _dispatcher.Send(new GetTeamScore()
                {
                    CompetitionId = (int)viewModel.EspnCompetitonId,
                    TeamId = awayTeam.EspnTeamId
                }, cancellationToken);

                viewModel.AwayTeam.Record = await _db.TeamSeasonRecords
                    .Where(tsr => tsr.Season.Year == year)
                    .Where(tsr => tsr.TeamId == viewModel.AwayTeam.Id)
                    .Select(tsr => tsr.Record)
                    .FirstAsync(cancellationToken);

                //Populate User Picks
                var userPicks = await _db.UserPicks
                    .Include(up => up.PickLineItems)
                    .Where(up => up.IsPaid)
                    .Where(up => up.PickLineItems.Any(up => up.CompetitionId == viewModel.CompetitionId))
                    .ToListAsync(cancellationToken);

                var userIds = userPicks.Select(up => up.UserId.ToString())
                    .Distinct()
                    .ToList();

                var users = await _db.Users
                    .Where(u => userIds.Contains(u.Id))
                    .Select(u => new { u.Id, u.UserName })
                    .ToListAsync(cancellationToken);

                var picks = new List<UserPickScoreViewModel>();

                var currentIndex = 0;
                var user = userPicks.FirstOrDefault().UserId;
                var teams = new List<Teams>()
                {
                    homeTeam, awayTeam
                };
                foreach (var pick in userPicks)
                {
                    if (pick.UserId == user)
                    {
                        currentIndex++;
                    }
                    else
                    {
                        currentIndex = 1;
                        user = pick.UserId;
                    }


                    var selectedTeamId = pick.PickLineItems.First(pl => pl.CompetitionId == viewModel.CompetitionId)
                        .PickTeamId;
                    var team = teams.Find(t => t.TeamsId == selectedTeamId);
                    picks.Add(new UserPickScoreViewModel()
                    {
                        SelectedTeam = team.FullName,
                        SelectedTeamAbbreviation = team.Abbreviation,
                        Username = users.Find(u => u.Id == pick.UserId.ToString()).UserName,
                        UserPickNumber = currentIndex,
                        SelectedTeamId = team.TeamsId
                    });
                }

                viewModel.UserPicks = picks;
            }
        }
    }
}