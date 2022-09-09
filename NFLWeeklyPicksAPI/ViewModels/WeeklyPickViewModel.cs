using NFLWeeklyPicksAPI.Models.Entities;
using System.Linq.Expressions;

namespace NFLWeeklyPicksAPI.ViewModels
{
    public class UserPicks
    {
        public int UserPickId { get; set; }
        public Guid UserId { get; set; }
        public int Season { get; set; }
        public int Week { get; set; }
        public IEnumerable<UserPickLineItems> PickLineItems { get; set; }

        internal static Expression<Func<Models.Entities.UserPicks, UserPicks>> Selector =>
            record => new UserPicks
            {
                UserPickId = record.UserPicksId,
                UserId = record.UserId,
                Season = record.Season,
                Week = record.Week,
                PickLineItems = record.PickLineItems.Select(pli => new UserPickLineItems
                {
                    CompetitionId = pli.CompetitionId,
                    PickTeamId = pli.PickTeamId,
                    PickTypeId = pli.PickTypeId,
                    TotalPoints = pli.PickPoints,
                    UserPickLineItemId = pli.UserPickLineItemId
                })
            };
    }

    public class UserPickLineItems
    {
        public int UserPickLineItemId { get; set; }
        public int PickTypeId { get; set; }
        public long CompetitionId { get; set; }
        public int PickTeamId { get; set; }
        public int TotalPoints { get; set; }
    }
}