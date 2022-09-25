using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NFLWeeklyPicksAPI.Extensions;
using NFLWeeklyPicksAPI.Models.Entities;
using NFLWeeklyPicksAPI.ViewModels;

namespace NFLWeeklyPicksAPI.Queries.SeasonWeeks
{
    public class ListSeasonWeeks : IRequest<IEnumerable<SeasonWeeksViewModel>>
    {
        public int Season { get; set; }

        public class Handler : IRequestHandler<ListSeasonWeeks, IEnumerable<SeasonWeeksViewModel>>
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

            public async Task<IEnumerable<SeasonWeeksViewModel>> Handle(ListSeasonWeeks request,
                CancellationToken cancellationToken)
            {
                var viewModels = await _db.SeasonWeeks
                    .Where(sw => sw.Season.Year == request.Season)
                    .OrderBy(sw => sw.StartDate)
                    .Select(SeasonWeeksViewModel.Selector)
                    .ToListAsync(cancellationToken);

                await SupplementSubmittedUserPicks(viewModels, cancellationToken);

                return viewModels;
            }

            private async Task SupplementSubmittedUserPicks(IEnumerable<SeasonWeeksViewModel> viewModels,
                CancellationToken cancellationToken)
            {
                var currentUser = await _contextAccessor.HttpContext.GetCurrentUser(_userManager);


                foreach (var viewModel in viewModels)
                {
                    //currentIndex++;
                    var userPicks = await _db.UserPicks
                        .Where(
                            u => u.Season == viewModel.Season && u.Week == viewModel.WeekNumber &&
                                 u.UserId == Guid.Parse(currentUser.Id))
                        .Select(u => u.UserPicksId)
                        .ToListAsync(cancellationToken);
                    var currentIndex = 0;
                    foreach (var userPick in userPicks)
                    {
                        currentIndex++;
                        viewModel.UserPicks.Add(new SeasonWeekUserPickViewModel()
                        {
                            UserPickId = userPick,
                            UserPickDescription = $"{currentUser} - {currentIndex}"
                        });
                    }
                }
            }
        }
    }
}