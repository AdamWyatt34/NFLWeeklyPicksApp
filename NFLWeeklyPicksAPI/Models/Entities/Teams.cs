namespace NFLWeeklyPicksAPI.Models.Entities;

public class Teams
{
    public int TeamsId { get; set; }
    public int EspnTeamId { get; set; }
    public string Location { get; set; }
    public string Nickname { get; set; }
    public string FullName { get; set; }
    public string LogoURL { get; set; }
    public string Abbreviation { get; set; }
}