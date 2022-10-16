using MediatR;
using Microsoft.EntityFrameworkCore;
using NFLWeeklyPicksAPI.ViewModels;

namespace NFLWeeklyPicksAPI.Queries.SeasonWeeks;

public class ListSeasons : IRequest<IEnumerable<SeasonViewModel>>
{
    public class Handler : IRequestHandler<ListSeasons, IEnumerable<SeasonViewModel>>
    {
        private readonly ApplicationDbContext _db;

        public Handler(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<SeasonViewModel>> Handle(ListSeasons request, CancellationToken cancellationToken)
        {
            return await _db.Seasons
                .Select(SeasonViewModel.Selector)
                .ToListAsync(cancellationToken);
        }
    }
}