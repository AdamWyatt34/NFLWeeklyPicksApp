using System.ComponentModel.DataAnnotations;

namespace NFLWeeklyPicksAPI.Models.Entities
{
    public enum PickType
    {
        Default = 2,
        WithPoints = 3
    }

    public class PickTypes
    {
        [Key]
        public int PickTypeId { get; set; }

        public PickType PickType { get; set; }
    }
}