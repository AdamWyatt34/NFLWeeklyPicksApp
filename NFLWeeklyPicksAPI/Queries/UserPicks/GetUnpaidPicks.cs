using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NFLWeeklyPicksAPI.ViewModels;

namespace NFLWeeklyPicksAPI.Queries.UserPicks;

public class GetUnpaidPicks : IRequest<IEnumerable<UnpaidPickViewModel>>
{
    [FromRoute] public int Season { get; set; }
    [FromRoute] public int Week { get; set; }

    public class Handler : IRequestHandler<GetUnpaidPicks, IEnumerable<UnpaidPickViewModel>>
    {
        private readonly ApplicationDbContext _db;

        public Handler(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<UnpaidPickViewModel>> Handle(GetUnpaidPicks request,
            CancellationToken cancellationToken)
        {
            var records = await _db.UserPicks
                .Where(u => u.Season == request.Season)
                .Where(u => u.Week == request.Week)
                .Where(u => !u.IsPaid)
                .Select(UnpaidPickViewModel.Selector)
                .ToListAsync(cancellationToken);

            //Supplement User Pick Description
            await SupplementViewModels(records, request, cancellationToken);

            return records;
        }

        public async Task SupplementViewModels(List<UnpaidPickViewModel> viewModels, GetUnpaidPicks request,
            CancellationToken cancellationToken)
        {
            // get all user picks for the week
            var allPicks = await _db.UserPicks
                .Where(u => u.Season == request.Season)
                .Where(u => u.Week == request.Week)
                .Select(UnpaidPickViewModel.Selector)
                .ToListAsync(cancellationToken);

            var userIds = allPicks.Select(up => up.UserId.ToString())
                .Distinct()
                .ToList();

            var users = await _db.Users
                .Where(u => userIds.Contains(u.Id))
                .Select(u => new { u.Id, u.UserName })
                .ToListAsync(cancellationToken);

            var currentIndex = 0;
            var userId = allPicks.FirstOrDefault().UserId;
            foreach (var pick in allPicks)
            {
                if (pick.UserId == userId)
                {
                    currentIndex++;
                }
                else
                {
                    currentIndex = 1;
                    userId = pick.UserId;
                }

                var user = users.Find(u => u.Id == pick.UserId.ToString()).UserName;
                pick.UserPickDescription = $"{user} - {currentIndex}";
            }

            foreach (var viewModel in viewModels)
            {
                viewModel.UserPickDescription =
                    allPicks.First(p => p.UserPickId == viewModel.UserPickId).UserPickDescription;
            }
        }
    }
}