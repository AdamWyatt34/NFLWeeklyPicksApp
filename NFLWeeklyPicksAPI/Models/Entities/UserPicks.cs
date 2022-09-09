using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NFLWeeklyPicksAPI.Models.Entities
{
    public class UserPicks
    {
        [Key] public int UserPicksId { get; set; }

        public Guid UserId { get; set; }
        public int Season { get; set; }
        public int Week { get; set; }
        public ICollection<UserPickLineItems> PickLineItems { get; set; }
    }
}