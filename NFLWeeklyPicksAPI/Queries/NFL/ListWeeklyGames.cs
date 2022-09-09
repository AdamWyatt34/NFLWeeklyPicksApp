using MediatR;
using Microsoft.AspNetCore.Mvc;
using NFLWeeklyPicksAPI.Extensions;
using NFLWeeklyPicksAPI.Models.ESPNDataModels;
using NFLWeeklyPicksAPI.ViewModels;

namespace NFLWeeklyPicksAPI.Queries.NFL
{
    public class ListWeeklyGames : IRequest<WeeklyGamesViewModel>
    {
        public int Season { get; set; }
        public int Week { get; set; }

        internal class Handler : IRequestHandler<ListWeeklyGames, WeeklyGamesViewModel>
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

            public async Task<WeeklyGamesViewModel> Handle(ListWeeklyGames request, CancellationToken cancellationToken)
            {
                var apiUrl = $"{_apiUrl}/seasons/{request.Season}/types/2/weeks/{request.Week}/events";

                var client = _httpClient.CreateClientWithUrl(apiUrl);

                var weeklyGames = await client.GetFromJsonAsync<WeeklyGameModel>(apiUrl, cancellationToken);

                List<WeeklyGameViewModel> games = new();

                weeklyGames.items.ForEach(i =>
                {
                    games.Add(_dispatcher.Send(new GetCompetitions()
                    {
                        Season = request.Season,
                        CompetitionUrls = i
                    }, cancellationToken).Result);
                });

                var result = new WeeklyGamesViewModel()
                {
                    Season = request.Season,
                    Week = request.Week,
                    Games = games
                };

                return result;
            }
        }
    }
}