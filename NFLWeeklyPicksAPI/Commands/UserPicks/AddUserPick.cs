using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NFLWeeklyPicksAPI.Extensions;
using NFLWeeklyPicksAPI.Models.Entities;

namespace NFLWeeklyPicksAPI.Commands
{
    public class AddUserPick : IRequest<int>
    {
        [FromBody] public ViewModels.UserPicks UserPick { get; set; }

        internal class Handler : IRequestHandler<AddUserPick, int>
        {
            private readonly ApplicationDbContext _db;
            private readonly IHttpContextAccessor _contextAccessor;
            private readonly UserManager<User> _userManager;

            public Handler(ApplicationDbContext db, IHttpContextAccessor contextAccessor, UserManager<User> userManager)
            {
                _db = db;
                _contextAccessor = contextAccessor;
                _userManager = userManager;
            }

            public async Task<int> Handle(AddUserPick request, CancellationToken cancellationToken)
            {
                var currentUser = await _contextAccessor.HttpContext.GetCurrentUser(_userManager);
                List<Models.Entities.UserPickLineItems> picks = new();

                foreach (var pick in request.UserPick.PickLineItems)
                {
                    var pickLine = new Models.Entities.UserPickLineItems
                    {
                        CompetitionId = pick.CompetitionId,
                        PickTeamId = pick.PickTeamId,
                        PickTypeId = pick.PickTypeId,
                        PickPoints = pick.TotalPoints
                    };

                    picks.Add(pickLine);
                }

                var result = await _db.UserPicks.AddAsync(new Models.Entities.UserPicks
                {
                    Season = request.UserPick.Season,
                    Week = request.UserPick.Week,
                    UserId = Guid.Parse(currentUser.Id),
                    PickLineItems = picks,
                }, cancellationToken);

                await _db.SaveChangesAsync(cancellationToken);

                return result.Entity.UserPicksId;
            }
        }

        public class Validator : AbstractValidator<AddUserPick>
        {
            public Validator()
            {
                RuleFor(request => request.UserPick.PickLineItems)
                    .NotEmpty()
                    .WithMessage("Can't be empty.");
            }
        }
    }
}