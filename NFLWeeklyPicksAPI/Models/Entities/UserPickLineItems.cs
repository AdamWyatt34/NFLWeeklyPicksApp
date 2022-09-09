using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NFLWeeklyPicksAPI.Models.Entities
{
    public class UserPickLineItems
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserPickLineItemId { get; set; }

        public int UserPickId { get; set; }
        public UserPicks UserPick { get; set; }
        public int PickTypeId { get; set; }
        public PickTypes PickType { get; set; }
        public long CompetitionId { get; set; }
        public int PickTeamId { get; set; }
        public int PickPoints { get; set; }
    }
}