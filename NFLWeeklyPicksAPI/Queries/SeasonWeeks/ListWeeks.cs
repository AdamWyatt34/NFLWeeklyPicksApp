using MediatR;
using Microsoft.EntityFrameworkCore;
using NFLWeeklyPicksAPI.ViewModels;

namespace NFLWeeklyPicksAPI.Queries.SeasonWeeks;

public class ListWeeks : IRequest<IEnumerable<WeekViewModel>>
{
    public int SeasonId { get; set; }

    public class Handler : IRequestHandler<ListWeeks, IEnumerable<WeekViewModel>>
    {
        private readonly ApplicationDbContext _db;

        public Handler(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<WeekViewModel>> Handle(ListWeeks request, CancellationToken cancellationToken)
        {
            return await _db.SeasonWeeks
                .Where(sw => sw.SeasonId == request.SeasonId)
                .Select(WeekViewModel.Selector)
                .ToListAsync(cancellationToken);
        }
    }
}