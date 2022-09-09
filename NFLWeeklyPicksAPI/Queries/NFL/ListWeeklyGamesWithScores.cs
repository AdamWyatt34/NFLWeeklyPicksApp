using MediatR;
using NFLWeeklyPicksAPI.Extensions;
using NFLWeeklyPicksAPI.Models.ESPNDataModels;
using NFLWeeklyPicksAPI.ViewModels;

namespace NFLWeeklyPicksAPI.Queries.NFL;

public class ListWeeklyGamesWithScores : IRequest<WeeklyGamesWithScoreViewModel>
{
    public int Season { get; set; }
    public int Week { get; set; }

    public class Handler : IRequestHandler<ListWeeklyGamesWithScores, WeeklyGamesWithScoreViewModel>
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

        public async Task<WeeklyGamesWithScoreViewModel> Handle(ListWeeklyGamesWithScores request,
            CancellationToken cancellationToken)
        {
            var apiUrl = $"{_apiUrl}/seasons/{request.Season}/types/2/weeks/{request.Week}/events";

            var client = _httpClient.CreateClientWithUrl(apiUrl);

            var weeklyGames = await client.GetFromJsonAsync<WeeklyGameModel>(apiUrl, cancellationToken);

            List<WeeklyGameWithScoreViewModel> games = new();

            foreach (var weeklyGame in weeklyGames.items)
            {
                games.Add(await _dispatcher.Send(new GetCompetitionsWithScores()
                {
                    Season = request.Season,
                    WeekNumber = request.Week,
                    CompetitionUrls = weeklyGame
                }, cancellationToken));
            }

            var result = new WeeklyGamesWithScoreViewModel()
            {
                Season = request.Season,
                Week = request.Week,
                Games = games
            };

            return result;
        }
    }
}