using Microsoft.AspNetCore.Components;
using NFLWeeklyPicksUI.Models;
using NFLWeeklyPicksUI.Services;
using Radzen;

namespace NFLWeeklyPicksUI.Pages.UnpaidPicks;

public partial class UnpaidPicksTable
{
    [Inject] public IUserPickService PickService { get; set; }
    [Inject] public ISeasonWeekService SeasonWeekService { get; set; }
    [Inject] public HttpClient Client { get; set; }
    [Inject] public NotificationService NotificationService { get; set; }
    private IEnumerable<SeasonViewModel> Seasons { get; set; }
    private IEnumerable<WeekViewModel> Weeks { get; set; } = new List<WeekViewModel>();
    private List<UnpaidPickViewModel> UnpaidPicks { get; set; } = new();
    private int SeasonId { get; set; }
    private int WeekId { get; set; }
    private bool _isBusy;


    protected override async Task OnInitializedAsync()
    {
        SeasonId = 1;
        Seasons = await SeasonWeekService.ListSeasons();
        Weeks = await SeasonWeekService.ListWeeks(1);
    }

    private async Task OnSeasonChange(object value, string name)
    {
        Weeks = await SeasonWeekService.ListWeeks(SeasonId);
    }

    private async Task OnWeekChange(object value, string name)
    {
        UnpaidPicks = await PickService.GetUnpaidPicks(
            Seasons.First(s => s.SeasonId == SeasonId).Year,
            Weeks.First(w => w.SeasonWeekId == WeekId).WeekNumber);
    }

    private static void MarkPickPaid(UnpaidPickViewModel viewModel)
    {
        viewModel.IsPaid = !viewModel.IsPaid;
    }

    private async void Submit()
    {
        _isBusy = true;
        var data = UnpaidPicks.Where(up => up.IsPaid);

        var result = await PickService.MarkUnpaidPicks(data);

        if (result)
        {
            NotificationService.Notify(new NotificationMessage
            {
                Severity = NotificationSeverity.Success,
                Summary = $"Picks updated successfully",
                Duration = 4000
            });
            _isBusy = false;
        }
        else
        {
            NotificationService.Notify(new NotificationMessage
            {
                Severity = NotificationSeverity.Error,
                Summary = "Error updating picks",
                Detail = "Please try again.",
                Duration = 4000
            });
            _isBusy = false;
        }

        _isBusy = false;
        StateHasChanged();
    }
}