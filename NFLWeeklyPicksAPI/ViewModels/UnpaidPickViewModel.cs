using System.Linq.Expressions;

namespace NFLWeeklyPicksAPI.ViewModels;

public class UnpaidPickViewModel
{
    public int UserPickId { get; set; }
    public Guid UserId { get; set; }
    public string UserPickDescription { get; set; }
    public bool IsPaid { get; set; }

    public static Expression<Func<Models.Entities.UserPicks, UnpaidPickViewModel>> Selector =>
        record => new UnpaidPickViewModel()
        {
            UserPickId = record.UserPicksId,
            UserId = record.UserId,
            IsPaid = record.IsPaid
        };
}