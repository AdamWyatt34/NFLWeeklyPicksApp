using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace NFLWeeklyPicksAPI.Queries.UserPicks
{
    public class GetUserPick : IRequest<ViewModels.UserPicks>
    {
        [FromRoute]
        public int UserPickId { get; set; }

        public class Handler : IRequestHandler<GetUserPick, ViewModels.UserPicks>
        {
            private readonly ApplicationDbContext _db;

            public Handler(ApplicationDbContext db)
            {
                _db = db;
            }

            public async Task<ViewModels.UserPicks> Handle(GetUserPick request, CancellationToken cancellationToken)
            {
                return await _db.UserPicks
                    .Where(up => up.UserPicksId == request.UserPickId)
                    .Select(ViewModels.UserPicks.Selector)
                    .FirstOrDefaultAsync(cancellationToken);
            }
        }
    }
}