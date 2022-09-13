using MediatR;
using Microsoft.EntityFrameworkCore;
using NFLWeeklyPicksAPI.Extensions;
using NFLWeeklyPicksAPI.Models.ESPNDataModels;
using NFLWeeklyPicksAPI.Models.ESPNSyncModels;

namespace NFLWeeklyPicksAPI.Commands.Teams;

public class SyncTeamsWithEspn : IRequest<Unit>
{
    public int Season { get; set; }

    public class Handler : IRequestHandler<SyncTeamsWithEspn, Unit>
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private readonly IMediator _dispatcher;
        private string _apiUrl;

        public Handler(ApplicationDbContext db, IHttpClientFactory clientFactory, IConfiguration configuration,
            IMediator dispatcher)
        {
            _db = db;
            _clientFactory = clientFactory;
            _configuration = configuration;
            _dispatcher = dispatcher;
            _apiUrl = _configuration.GetValue<string>("ESPNAPIUrl");
        }

        public async Task<Unit> Handle(SyncTeamsWithEspn request, CancellationToken cancellationToken)
        {
            var apiUrl = $"{_apiUrl}/seasons/{request.Season}/teams?lang=en&region=us";
            var client = _clientFactory.CreateClientWithUrl(apiUrl);

            var result = await client.GetFromJsonAsync<TeamSyncModel>(apiUrl, cancellationToken: cancellationToken);

            var teamUrls = result!.items;
            var pages = result.pageCount;

            //Get all team urls
            for (var i = 2; i <= pages; i++)
            {
                var pageResult =
                    await client.GetFromJsonAsync<TeamSyncModel>(apiUrl + "&page=" + i,
                        cancellationToken: cancellationToken);
                teamUrls.AddRange(pageResult!.items);
            }

            //get team model from url;
            foreach (var teamUrl in teamUrls)
            {
                var teamClient = _clientFactory.CreateClientWithUrl(teamUrl.Ref);
                var team = await teamClient.GetFromJsonAsync<TeamModel>(teamUrl.Ref,
                    cancellationToken: cancellationToken);

                //Upsert record
                await UpsertTeam(team, cancellationToken);

                await _db.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }

        private async Task UpsertTeam(TeamModel team, CancellationToken cancellationToken)
        {
            var record = await _db.Teams
                .Where(t => t.EspnTeamId == int.Parse(team.id))
                .FirstOrDefaultAsync(cancellationToken);

            if (record == null)
            {
                await _db.Teams.AddAsync(new Models.Entities.Teams()
                {
                    EspnTeamId = int.Parse(team.id),
                    Abbreviation = team.abbreviation,
                    FullName = team.displayName,
                    Location = team.location,
                    LogoURL = team.logos.First().href,
                    Nickname = team.nickname
                }, cancellationToken);
            }
            else
            {
                record.Abbreviation = team.abbreviation;
                record.FullName = team.displayName;
                record.Location = team.location;
                record.LogoURL = team.logos.First().href;
                record.Nickname = team.nickname;
            }
        }
    }
}