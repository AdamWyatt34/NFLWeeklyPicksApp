using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace NFLWeeklyPicksAPI.Commands;

public class UpdateUserPick : IRequest<Unit>
{
    [FromBody] public ViewModels.UserPicks UserPick { get; set; }

    public class Handler : IRequestHandler<UpdateUserPick, Unit>
    {
        private readonly ApplicationDbContext _db;

        public Handler(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Unit> Handle(UpdateUserPick request, CancellationToken cancellationToken)
        {
            var currentPick = await _db.UserPicks
                .Include(up => up.PickLineItems)
                .Where(up => up.UserPicksId == request.UserPick.UserPickId)
                .FirstAsync(cancellationToken);

            List<Models.Entities.UserPickLineItems> picks = new();
            foreach (var pick in request.UserPick.PickLineItems)
            {
                var competition = await _db.Competitions.FirstAsync(c => c.CompetitionsId == pick.CompetitionId,
                    cancellationToken: cancellationToken);
                var pickLine = new Models.Entities.UserPickLineItems
                {
                    CompetitionId = pick.CompetitionId,
                    Competition = competition,
                    PickTeamId = pick.PickTeamId,
                    PickTypeId = pick.PickTypeId,
                    PickPoints = pick.TotalPoints
                };

                picks.Add(pickLine);
            }

            currentPick.PickLineItems.Clear();
            currentPick.PickLineItems = picks;

            await _db.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}