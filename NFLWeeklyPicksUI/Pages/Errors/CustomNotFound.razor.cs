using Microsoft.AspNetCore.Components;

namespace NFLWeeklyPicksUI.Pages.Errors;

public partial class CustomNotFound
{
    [Inject] public NavigationManager NavigationManager { get; set; }

    public void NavigateToHome()
    {
        NavigationManager.NavigateTo("/");
    }
}