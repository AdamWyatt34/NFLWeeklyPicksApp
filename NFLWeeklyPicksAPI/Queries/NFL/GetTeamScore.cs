using MediatR;
using NFLWeeklyPicksAPI.Models.ESPNDataModels;

namespace NFLWeeklyPicksAPI.Queries.NFL;

public class GetTeamScore : IRequest<double>
{
    public int CompetitionId { get; set; }
    public int TeamId { get; set; }

    public class Handler : IRequestHandler<GetTeamScore, double>
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly string _apiUrl;

        public Handler(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _apiUrl = _configuration.GetValue<string>("ESPNAPIUrl");
        }

        public async Task<double> Handle(GetTeamScore request, CancellationToken cancellationToken)
        {
            var apiUrl =
                $"{_apiUrl}/events/{request.CompetitionId}/competitions/{request.CompetitionId}/competitors/{request.TeamId}/score";
            var client = _httpClientFactory.CreateClient(apiUrl);

            var scoreModel = await client.GetFromJsonAsync<ScoreModel>(apiUrl, cancellationToken);

            return scoreModel.value;
        }
    }
}