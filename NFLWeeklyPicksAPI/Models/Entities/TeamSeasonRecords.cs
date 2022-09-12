namespace NFLWeeklyPicksAPI.Models.Entities;

public class TeamSeasonRecords
{
    public int TeamSeasonRecordsId { get; set; }
    public int TeamId { get; set; }
    public Teams Team { get; set; }
    public int SeasonId { get; set; }
    public Season Season { get; set; }
    public string Record { get; set; }
}