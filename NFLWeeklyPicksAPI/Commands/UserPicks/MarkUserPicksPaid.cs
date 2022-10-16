using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace NFLWeeklyPicksAPI.Commands.UserPicks;

public class MarkUserPicksPaid : IRequest<Unit>
{
    public IEnumerable<UserPaidModel> Picks { get; set; }
    //[FromBody] public Model Body { get; set; }

    public record Model(IEnumerable<UserPaidModel> Picks);

    public record UserPaidModel(int UserPickId, bool IsPaid);

    public class Handler : IRequestHandler<MarkUserPicksPaid, Unit>
    {
        private readonly ApplicationDbContext _db;

        public Handler(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Unit> Handle(MarkUserPicksPaid request, CancellationToken cancellationToken)
        {
            foreach (var pick in request.Picks)
            {
                var userPick = await _db.UserPicks.FirstAsync(p => p.UserPicksId == pick.UserPickId, cancellationToken);
                userPick.IsPaid = pick.IsPaid;
            }

            await _db.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}