namespace NFLWeeklyPicksAPI.Models.Entities;

public class Competitions
{
    public int CompetitionsId { get; set; }
    public long EspnCompetitionId { get; set; }
    public int SeasonWeeksId { get; set; }
    public SeasonWeeks SeasonWeeks { get; set; }
    public string GameName { get; set; }
    public DateTime GameDate { get; set; }
    public int HomeTeamId { get; set; }
    public Teams HomeTeam { get; set; }
    public int AwayTeamId { get; set; }
    public Teams AwayTeam { get; set; }
    public string Odds { get; set; }
    public string HomeTeamScoreUrl { get; set; }
    public string AwayTeamScoreUrl { get; set; }
}