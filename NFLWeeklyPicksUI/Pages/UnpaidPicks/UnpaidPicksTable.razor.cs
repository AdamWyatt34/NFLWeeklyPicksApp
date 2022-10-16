using Microsoft.AspNetCore.Components;
using NFLWeeklyPicksUI.Models;
using NFLWeeklyPicksUI.Services;
using Radzen;

namespace NFLWeeklyPicksUI.Pages.UnpaidPicks;

public partial class UnpaidPicksTable
{
    [Inject] public IUserPickService PickService { get; set; }
    [Inject] public HttpClient Client { get; set; }
    [Inject] public NotificationService NotificationService { get; set; }
    private List<UnpaidPickViewModel> UnpaidPicks { get; set; }


    protected override Task OnInitializedAsync()
    {
        return Task.CompletedTask;
    }
}