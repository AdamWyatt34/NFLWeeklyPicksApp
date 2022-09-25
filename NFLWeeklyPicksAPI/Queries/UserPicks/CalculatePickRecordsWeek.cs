using MediatR;
using Microsoft.EntityFrameworkCore;
using NFLWeeklyPicksAPI.Extensions;
using NFLWeeklyPicksAPI.Models.ESPNDataModels;
using NFLWeeklyPicksAPI.Queries.NFL;
using NFLWeeklyPicksAPI.Queries.SeasonWeeks;
using NFLWeeklyPicksAPI.ViewModels;

namespace NFLWeeklyPicksAPI.Queries.UserPicks;

public class CalculatePickRecordsWeek : IRequest<List<UserPickWeeklyRecordViewModel>>
{
    public int Season { get; set; }
    public int WeekNumber { get; set; }

    public class Handler : IRequestHandler<CalculatePickRecordsWeek, List<UserPickWeeklyRecordViewModel>>
    {
        private readonly IMediator _dispatcher;
        private readonly ApplicationDbContext _db;

        public Handler(IMediator dispatcher, ApplicationDbContext db)
        {
            _dispatcher = dispatcher;
            _db = db;
        }

        public async Task<List<UserPickWeeklyRecordViewModel>> Handle(CalculatePickRecordsWeek request,
            CancellationToken cancellationToken)
        {
            var weeklyGames = await _dispatcher.Send(new ListWeeklyGamesWithScore()
            {
                Season = request.Season,
                Week = request.WeekNumber
            }, cancellationToken);

            List<WeeklyGameWithScoreAndWinnerViewModel> games = new();

            foreach (var weeklyGame in weeklyGames.Games)
            {
                var game = new WeeklyGameWithScoreAndWinnerViewModel()
                {
                    AwayTeam = weeklyGame.AwayTeam,
                    CompetitionId = weeklyGame.CompetitionId,
                    EspnCompetitonId = weeklyGame.EspnCompetitonId,
                    GameDate = weeklyGame.GameDate,
                    GameName = weeklyGame.GameName,
                    HomeTeam = weeklyGame.HomeTeam,
                    Odds = weeklyGame.Odds,
                };

                var userPicks = await _db.UserPicks
                    .Include(up => up.PickLineItems)
                    .Where(up => up.Season == request.Season)
                    .Where(up => up.Week == request.WeekNumber)
                    .Where(up => up.PickLineItems.Any(up => up.CompetitionId == game.CompetitionId))
                    .ToListAsync(cancellationToken);

                var userIds = userPicks.Select(up => up.UserId.ToString())
                    .Distinct()
                    .ToList();

                var users = await _db.Users
                    .Where(u => userIds.Contains(u.Id))
                    .Select(u => new { u.Id, u.UserName })
                    .ToListAsync(cancellationToken);

                var picks = new List<UserPickScoreViewModelWithWinner>();

                var currentIndex = 0;
                var user = userPicks.FirstOrDefault().UserId;
                //Should be able to change loop just to populate is correct
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

                    var selectedTeamId = pick.PickLineItems.First(pl => pl.CompetitionId == game.CompetitionId)
                        .PickTeamId;
                    var team = game.HomeTeam.Id == selectedTeamId ? game.HomeTeam : game.AwayTeam;
                    picks.Add(new UserPickScoreViewModelWithWinner()
                    {
                        UserPickId = pick.UserPicksId,
                        SelectedTeam = team.FullName,
                        SelectedTeamAbbreviation = team.Abbreviation,
                        Username = users.Find(u => u.Id == pick.UserId.ToString()).UserName,
                        UserPickNumber = currentIndex,
                        SelectedTeamId = team.Id,
                        IsCorrect = selectedTeamId == game.WinningTeamId
                    });
                }

                game.UserPicks = picks;

                games.Add(game);
            }

            //All games with each user being right or wrong.
            var output = new List<UserPickWeeklyRecordViewModel>();
            var distinctUserPicks = games.SelectMany(g => g.UserPicks)
                .Select(u => new { u.UserPickId, u.Username, u.UserPickNumber })
                .Distinct()
                .ToList();

            foreach (var userPick in distinctUserPicks)
            {
                var usersPicks = games
                    .SelectMany(g => g.UserPicks)
                    .Where(up => up.UserPickId == userPick.UserPickId);

                var userPickScoreViewModelWithWinners = usersPicks.ToList();
                output.Add(new UserPickWeeklyRecordViewModel()
                {
                    UserPickId = userPick.UserPickId,
                    UserPickDescription = $"{userPick.Username} - {userPick.UserPickNumber}",
                    Losses = userPickScoreViewModelWithWinners.Count(up => !up.IsCorrect).ToString(),
                    Wins = userPickScoreViewModelWithWinners.Count(up => up.IsCorrect).ToString()
                });
            }

            return output;
        }
    }
}