using Microsoft.AspNetCore.Components;
using NFLWeeklyPicksUI.Models;
using NFLWeeklyPicksUI.Services;

namespace NFLWeeklyPicksUI.Pages.PickScores;

public partial class WeeklyGamesWithScoresPage
{
    [Parameter] public int Season { get; set; }
    [Parameter] public int Week { get; set; }
    [Inject] public IUserPickService UserPickService { get; set; }

    private WeeklyGamesWithScoreViewModel _gamesWithScores;
    private string _headerText = string.Empty;
    private bool _isLoading { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _isLoading = true;
        _gamesWithScores = await UserPickService.GetPicksWithScores(Season, Week);
        _headerText = $"{Season} Season Week {Week} Submitted Picks";
        _isLoading = false;
    }
}