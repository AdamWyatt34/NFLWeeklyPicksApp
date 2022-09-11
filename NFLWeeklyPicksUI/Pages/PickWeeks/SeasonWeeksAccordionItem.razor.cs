using Microsoft.AspNetCore.Components;
using NFLWeeklyPicksUI.Models;

namespace NFLWeeklyPicksUI.Pages.PickWeeks;

public partial class SeasonWeeksAccordionItem
{
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Parameter] public SeasonWeeksViewModel SeasonWeek { get; set; }

    private string AccordionText;
    private bool AddPickVisible;

    protected override void OnInitialized()
    {
        AccordionText =
            $"{SeasonWeek.WeekDescription} Start: {SeasonWeek.StartDate.ToShortDateString()}" +
            $" End: {SeasonWeek.EndDate.ToShortDateString()}";

        AddPickVisible = SeasonWeek.UserPicks.Count < 2; //TODO: Inject this
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