using MediatR;
using NFLWeeklyPicksAPI.Extensions;
using NFLWeeklyPicksAPI.Models.ESPNDataModels;
using NFLWeeklyPicksAPI.ViewModels;

namespace NFLWeeklyPicksAPI.Queries.NFL;

public class GetTeamsWithScore : IRequest<List<TeamWithScoreViewModel>>
{
    public int CompetitionId { get; set; }
    public int Season { get; set; }
    public List<int> TeamIds { get; set; }

    public class Handler : IRequestHandler<GetTeamsWithScore, List<TeamWithScoreViewModel>>
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IMediator _dispatcher;
        private readonly string _apiUrl;

        public Handler(IHttpClientFactory httpClient, IConfiguration configuration, IMediator dispatcher)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _dispatcher = dispatcher;
            _apiUrl = _configuration.GetValue<string>("ESPNAPIUrl");
        }

        public async Task<List<TeamWithScoreViewModel>> Handle(GetTeamsWithScore request,
            CancellationToken cancellationToken)
        {
            List<TeamWithScoreViewModel> result = new();

            foreach (var teamId in request.TeamIds)
            {
                var apiUrl = $"{_apiUrl}/seasons/{request.Season}/teams/{teamId}?lang=en&region=us";
                var client = _httpClient.CreateClientWithUrl(apiUrl);

                var team = await client.GetFromJsonAsync<TeamModel>(apiUrl, cancellationToken: cancellationToken);

                var record = await _dispatcher.Send(new GetRecord() { RecordURL = team.record.Ref }, cancellationToken);
                var score = await _dispatcher.Send(new GetTeamScore()
                    { CompetitionId = request.CompetitionId, TeamId = teamId }, cancellationToken);

                result.Add(new TeamWithScoreViewModel()
                {
                    Id = teamId,
                    Location = team.location,
                    Nickname = team.nickname,
                    FullName = team.displayName,
                    LogoURL = team.logos.First().href,
                    Record = record,
                    Abbreviation = team.abbreviation,
                    Score = score
                });
            }


            return result;
        }
    }
}