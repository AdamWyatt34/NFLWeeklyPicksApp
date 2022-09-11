namespace NFLWeeklyPicksAPI.ViewModels;

public class UserPickWeeklyRecordViewModel
{
    public int UserPickId { get; set; }
    public string UserPickDescription { get; set; }
    public string Wins { get; set; }
    public string Losses { get; set; }
    public string Record => $"{Wins} - {Losses}";
}