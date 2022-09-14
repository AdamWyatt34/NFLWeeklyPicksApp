using MediatR;
using Microsoft.EntityFrameworkCore;
using NFLWeeklyPicksAPI.Extensions;
using NFLWeeklyPicksAPI.Models.ESPNSyncModels;

namespace NFLWeeklyPicksAPI.Commands.SeasonWeeks;

public class SyncSeasonWeeks : IRequest<Unit>
{
    public class Handler : IRequestHandler<SyncSeasonWeeks, Unit>
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private string _apiUrl;

        public Handler(ApplicationDbContext db, IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _db = db;
            _clientFactory = clientFactory;
            _configuration = configuration;
            _apiUrl = _configuration.GetValue<string>("ESPNAPIUrl");
        }

        public async Task<Unit> Handle(SyncSeasonWeeks request, CancellationToken cancellationToken)
        {
            //Get current season from base url
            var seasonClient = _clientFactory.CreateClientWithUrl(_apiUrl);
            var seasonModel = await seasonClient.GetFromJsonAsync<CurrentSeasonModel>(_apiUrl, cancellationToken);

            var currentSeason = seasonModel.season.year;

            //Get the last week for current season from SeasonWeeks
            var lastSeasonWeek = await _db.SeasonWeeks
                .Where(sw => sw.Season.Year == currentSeason)
                .OrderByDescending(sw => sw.EndDate)
                .FirstAsync(cancellationToken);

            var weekListClient = _clientFactory.CreateClientWithUrl(seasonModel.season.type.weeks.Ref);
            var weekListModel =
                await weekListClient.GetFromJsonAsync<TeamSyncModel>(seasonModel.season.type.weeks.Ref,
                    cancellationToken);

            var nextWeekNumber = lastSeasonWeek.WeekNumber + 1;

            //Verify WeekNumber + 1 is a valid url
            if (nextWeekNumber > weekListModel.count)
                return Unit.Value;

            //Get Next Week Info
            var weekClient = _clientFactory.CreateClientWithUrl(weekListModel.items[nextWeekNumber - 1].Ref);
            var weekModel =
                await weekClient.GetFromJsonAsync<WeekSyncModel>(weekListModel.items[nextWeekNumber - 1].Ref,
                    cancellationToken: cancellationToken);

            //Get season Id
            var season = await _db.Seasons.FirstAsync(s => s.Year == currentSeason, cancellationToken);

            await _db.SeasonWeeks.AddAsync(new Models.Entities.SeasonWeeks()
            {
                EndDate = DateTime.Parse(weekModel.endDate),
                Season = season,
                StartDate = DateTime.Parse(weekModel.startDate),
                WeekDescription = weekModel.text,
                WeekNumber = weekModel.number
            }, cancellationToken);

            await _db.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}