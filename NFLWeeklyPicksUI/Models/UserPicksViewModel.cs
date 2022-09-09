using NFLWeeklyPicksUI.Enums;

namespace NFLWeeklyPicksUI.Models
{
    public class UserPicksRequestObject
    {
        public UserPicks UserPick { get; set; }
    }

    public class UserPicks
    {
        public int UserPickId { get; set; }
        public Guid UserId { get; set; }
        public int Season { get; set; }
        public int Week { get; set; }
        public IList<UserPickLineItems> PickLineItems { get; set; } = new List<UserPickLineItems>();
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