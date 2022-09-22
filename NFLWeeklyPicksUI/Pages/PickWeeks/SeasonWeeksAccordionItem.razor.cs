using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using NFLWeeklyPicksUI.Models;
using NFLWeeklyPicksUI.Options;

namespace NFLWeeklyPicksUI.Pages.PickWeeks;

public partial class SeasonWeeksAccordionItem
{
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public IOptions<PickOptions> PickOptions { get; set; }
    [Parameter] public SeasonWeeksViewModel SeasonWeek { get; set; }

    private PickOptions _pickOptions;
    private string AccordionText;
    private bool AddPickVisible;

    protected override void OnInitialized()
    {
        _pickOptions = PickOptions.Value;

        AccordionText =
            $"{SeasonWeek.WeekDescription} Start: {SeasonWeek.StartDate.ToShortDateString()}" +
            $" End: {SeasonWeek.EndDate.ToShortDateString()}";

        AddPickVisible = SeasonWeek.UserPicks.Count < _pickOptions.MaxPicks;
    }

    private void OpenWeekPicks(SeasonWeekUserPickViewModel viewModel)
    {
        NavigationManager.NavigateTo($"/picks/{viewModel.UserPickId}");
    }

    private void AddNewPick()
    {
        NavigationManager.NavigateTo($"picks/{SeasonWeek.Season}/{SeasonWeek.WeekNumber}");
    }

    private void OpenWeekPicksWithScore()
    {
        NavigationManager.NavigateTo($"picks/scores/{SeasonWeek.Season}/{SeasonWeek.WeekNumber}");
    }
}