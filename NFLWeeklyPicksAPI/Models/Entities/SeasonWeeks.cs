namespace NFLWeeklyPicksAPI.Models.Entities
{
    public class SeasonWeeks
    {
        public int SeasonWeeksId { get; set; }
        public int WeekNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string WeekDescription { get; set; }
        public int SeasonId { get; set; }
        public Season Season { get; set; }
    }
}