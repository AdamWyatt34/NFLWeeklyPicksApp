namespace NFLWeeklyPicksUI.Models;

public class UnpaidPickViewModel
{
    public int UserPickId { get; set; }
    public Guid UserId { get; set; }
    public string UserPickDescription { get; set; }
    public bool IsPaid { get; set; }
}