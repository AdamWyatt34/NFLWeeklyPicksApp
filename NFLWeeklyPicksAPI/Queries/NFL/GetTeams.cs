using MediatR;
using NFLWeeklyPicksAPI.Extensions;
using NFLWeeklyPicksAPI.Models.ESPNDataModels;
using NFLWeeklyPicksAPI.ViewModels;

namespace NFLWeeklyPicksAPI.Queries.NFL
{
    public class GetTeams : IRequest<List<TeamViewModel>>
    {
        public int Season { get; set; }
        public List<int> TeamIds { get; set; }

        internal class Handler : IRequestHandler<GetTeams, List<TeamViewModel>>
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

            public async Task<List<TeamViewModel>> Handle(GetTeams request, CancellationToken cancellationToken)
            {
                List<TeamViewModel> result = new();

                request.TeamIds.ForEach(id =>
                {
                    var apiUrl = $"{_apiUrl}/seasons/{request.Season}/teams/{id}?lang=en&region=us";
                    var client = _httpClient.CreateClientWithUrl(apiUrl);

                    var team = client.GetFromJsonAsync<TeamModel>(apiUrl).Result;

                    var record = _dispatcher.Send(new GetRecord() { RecordURL = team.record.Ref }).Result;

                    result.Add(new TeamViewModel
                    {
                        Id = id,
                        Location = team.location,
                        Nickname = team.nickname,
                        FullName = team.displayName,
                        LogoURL = team.logos.First().href,
                        Record = record,
                        Abbreviation = team.abbreviation
                    });
                });

                return result;

            }
        }
    }
}
