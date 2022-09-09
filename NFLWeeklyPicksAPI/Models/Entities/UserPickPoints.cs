using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NFLWeeklyPicksAPI.Models.Entities
{
    public class UserPickPoints
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserPickPointsId { get; set; }

        public int UserPickLineItemId { get; set; }
        public UserPickLineItems UserPickLineItem { get; set; }
        public int Points { get; set; }
    }
}