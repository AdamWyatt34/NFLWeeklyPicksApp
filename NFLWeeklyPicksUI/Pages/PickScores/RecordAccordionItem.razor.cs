using Microsoft.AspNetCore.Components;
using NFLWeeklyPicksUI.Models;
using NFLWeeklyPicksUI.Services;

namespace NFLWeeklyPicksUI.Pages.PickScores;

public partial class RecordAccordionItem
{
    [Parameter] public int Season { get; set; }
    [Parameter] public int Week { get; set; }
    [Inject] public IUserPickService PickService { get; set; }
    private List<UserPickWeeklyRecordViewModel> _userRecords;
    private string _headerText = string.Empty;
    private bool _isLoading;

    protected override async Task OnInitializedAsync()
    {
        _isLoading = true;
        _userRecords = await PickService.GetUserRecords(Season, Week);
        _headerText = $"User Records for Week {Week}";
        _isLoading = false;
    }
}