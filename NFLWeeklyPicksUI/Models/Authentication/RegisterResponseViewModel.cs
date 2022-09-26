namespace NFLWeeklyPicksUI.Models.Authentication;

public class RegisterResponseViewModel
{
    public bool Succeeded { get; set; }
    public IEnumerable<RegisterErrorViewModel> Errors { get; set; }
}

public class RegisterErrorViewModel
{
    public string Code { get; set; }
    public string Description { get; set; }
}