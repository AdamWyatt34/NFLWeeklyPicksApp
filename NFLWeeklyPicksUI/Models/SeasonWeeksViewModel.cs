namespace NFLWeeklyPicksUI.Models
{
    public class SeasonWeeksViewModel
    {
        public int SeasonWeeksId { get; set; }
        public int WeekNumber { get; set; }
        public string WeekDescription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsDisabled { get; set; }
        public int UserPickId { get; set; }
        public int Season { get; set; }
    }
}