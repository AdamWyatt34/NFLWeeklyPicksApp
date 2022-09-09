namespace NFLWeeklyPicksAPI.Models.Entities
{
    public class Season
    {
        public int SeasonId { get; set; }
        public int Type { get; set; }
        public int Year { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<SeasonWeeks> Weeks { get; set; }
    }
}