using MediatR;
using NFLWeeklyPicksAPI.Extensions;
using NFLWeeklyPicksAPI.Models.ESPNDataModels;

namespace NFLWeeklyPicksAPI.Queries.NFL
{
    public class GetCompetitionsOdds : IRequest<string>
    {
        public int CompetitoinId { get; set; }

        internal class Handler : IRequestHandler<GetCompetitionsOdds, string>
        {
            private readonly IHttpClientFactory _httpClient;
            private readonly IConfiguration _configuration;
            private readonly string _apiUrl;

            public Handler(IHttpClientFactory httpClient, IConfiguration configuration)
            {
                _httpClient = httpClient;
                _configuration = configuration;
                _apiUrl = _configuration.GetValue<string>("ESPNAPIUrl");
            }

            public async Task<string> Handle(GetCompetitionsOdds request, CancellationToken cancellationToken)
            {
                var apiUrl = $"{_apiUrl}/events/{request.CompetitoinId}/competitions/{request.CompetitoinId}/odds?lang=en&region=us";
                var client = _httpClient.CreateClientWithUrl(apiUrl);

                var result = await client.GetFromJsonAsync<CompetitionOddsModel>(apiUrl, cancellationToken);

                return result.items.First().details;
            }
        }
    }
}
