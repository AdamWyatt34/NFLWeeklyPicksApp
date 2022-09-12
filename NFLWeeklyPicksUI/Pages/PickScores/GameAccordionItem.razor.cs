using Microsoft.AspNetCore.Components;
using NFLWeeklyPicksUI.Models;
using Radzen;

namespace NFLWeeklyPicksUI.Pages.PickScores;

public partial class GameAccordionItem
{
    [Parameter] public WeeklyGameWithScoreViewModel GameWithScore { get; set; }
    private string _accordionText = string.Empty;
    private string _abbreviatedAccordionText = string.Empty;
    private bool _renderFullTeamNames;
    private string _winningTeam = string.Empty;

    protected override void OnInitialized()
    {
        _accordionText = $"{GameWithScore.AwayTeam.FullName}({GameWithScore.AwayTeam.Score})" +
                         $" @ {GameWithScore.HomeTeam.FullName}({GameWithScore.HomeTeam.Score})";
        _abbreviatedAccordionText =
            $"{GameWithScore.AwayTeam.Abbreviation}({GameWithScore.AwayTeam.Score})" +
            $" @ {GameWithScore.HomeTeam.Abbreviation}({GameWithScore.HomeTeam.Score})";

        _winningTeam = GameWithScore.HomeTeam.Score == GameWithScore.AwayTeam.Score
            ? "Tie"
            : GameWithScore.HomeTeam.Score > GameWithScore.AwayTeam.Score
                ? GameWithScore.HomeTeam.FullName
                : GameWithScore.AwayTeam.FullName;
    }

    private void RowRender(RowRenderEventArgs<UserPickScoreViewModel> args)
    {
        var classToAdd = args.Data.SelectedTeam == _winningTeam ? "row-highlight" : "";

        args.Attributes.Add("class", classToAdd);
    }

    private void OnChange(bool matches)
    {
        _renderFullTeamNames = !matches;
    }
}