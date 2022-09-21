using MediatR;
using Microsoft.EntityFrameworkCore;
using NFLWeeklyPicksAPI.Extensions;
using NFLWeeklyPicksAPI.Models.ESPNDataModels;
using NFLWeeklyPicksAPI.Queries.NFL;
using NFLWeeklyPicksAPI.ViewModels;

namespace NFLWeeklyPicksAPI.Queries.UserPicks;

public class CalculatePickRecordsWeek : IRequest<List<UserPickWeeklyRecordViewModel>>
{
    public int Season { get; set; }
    public int WeekNumber { get; set; }

    public class Handler : IRequestHandler<CalculatePickRecordsWeek, List<UserPickWeeklyRecordViewModel>>
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

        public async Task<List<UserPickWeeklyRecordViewModel>> Handle(CalculatePickRecordsWeek request,
            CancellationToken cancellationToken)
        {
            var apiUrl = $"{_apiUrl}/seasons/{request.Season}/types/2/weeks/{request.WeekNumber}/events";

            var client = _httpClient.CreateClientWithUrl(apiUrl);

            var weeklyGames = await client.GetFromJsonAsync<WeeklyGameModel>(apiUrl, cancellationToken);

            List<WeeklyGameWithScoreAndWinnerViewModel> games = new();

            foreach (var weeklyGame in weeklyGames.items)
            {
                var gameWithScore = await _dispatcher.Send(new GetCompetitionsWithScores()
                {
                    Season = request.Season,
                    WeekNumber = request.WeekNumber,
                    CompetitionUrls = weeklyGame
                }, cancellationToken);

                var game = new WeeklyGameWithScoreAndWinnerViewModel()
                {
                    AwayTeam = gameWithScore.AwayTeam,
                    CompetitionId = gameWithScore.CompetitionId,
                    GameDate = gameWithScore.GameDate,
                    GameName = gameWithScore.GameName,
                    HomeTeam = gameWithScore.HomeTeam,
                    Odds = gameWithScore.Odds,
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

                //Verify can replace all code above with dispatch to New ListWeeklyGamesWithScore
                
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

                output.Add(new UserPickWeeklyRecordViewModel()
                {
                    UserPickId = userPick.UserPickId,
                    UserPickDescription = $"{userPick.Username} - {userPick.UserPickNumber}",
                    Losses = usersPicks.Count(up => !up.IsCorrect).ToString(),
                    Wins = usersPicks.Count(up => up.IsCorrect).ToString()
                });
            }

            return output;
        }
    }
}