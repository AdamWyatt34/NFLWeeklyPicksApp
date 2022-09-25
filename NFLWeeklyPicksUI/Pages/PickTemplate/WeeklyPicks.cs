using Microsoft.AspNetCore.Components;
using NFLWeeklyPicksUI.Enums;
using NFLWeeklyPicksUI.Models;
using Radzen;
using System.Net.Http.Json;
using System.Threading;
using NFLWeeklyPicksUI.Services;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NFLWeeklyPicksUI.Pages.PickTemplate
{
    public partial class WeeklyPicks
    {
        [Inject] public HttpClient Client { get; set; }

        [Inject] public NotificationService NotificationService { get; set; }

        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public ISeasonWeekService SeasonWeekService { get; set; }
        [Parameter] public int? UserPickId { get; set; }

        [Parameter] public int? Season { get; set; }

        [Parameter] public int? Week { get; set; }

        private WeeklyGamesViewModel _games;
        private UserPicks _pick = new();
        private WeeklyGameViewModel _lastGame;
        private UserPickLineItems _lastPickLineItem;
        private bool _isLocked;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

#pragma warning disable CS8601 // Possible null reference assignment.
#pragma warning disable CS8602 // Dereference of a possibly null reference.

        protected override async Task OnInitializedAsync()
        {
            if (UserPickId.HasValue)
            {
                _pick = await Client.GetFromJsonAsync<UserPicks>($"api/user-pick/{UserPickId}/");
                _games = await SeasonWeekService.ListWeeklyGames(_pick.Season, _pick.Week);
            }
            else
            {
                _games = await SeasonWeekService.ListWeeklyGames(Season.Value, Week.Value);
            }

            //Change _isLocked to false for testing
            //_isLocked = _games.Games.First().GameDate <= DateTime.Now;
            _isLocked = false;
            _lastGame = _games.Games.Last();
            _games.Games.ForEach(game =>
            {
                var pickLineInfo =
                    _pick.PickLineItems.FirstOrDefault(pli => pli.CompetitionId == game.EspnCompetitionId);
                if (game.Equals(_lastGame))
                {
                    _pick.PickLineItems.Add(new UserPickLineItems
                    {
                        CompetitionId = game.EspnCompetitionId,
                        PickTypeId = (int)PickType.WithPoints,
                        PickTeamId = pickLineInfo?.PickTeamId ?? 0,
                        TotalPoints = pickLineInfo?.TotalPoints ?? 0,
                        UserPickLineItemId = pickLineInfo?.UserPickLineItemId ?? 0
                    });
                }
                else
                {
                    _pick.PickLineItems.Add(new UserPickLineItems
                    {
                        CompetitionId = game.EspnCompetitionId,
                        PickTypeId = (int)PickType.Default,
                        PickTeamId = pickLineInfo?.PickTeamId ?? 0,
                        TotalPoints = pickLineInfo?.TotalPoints ?? 0,
                        UserPickLineItemId = pickLineInfo?.UserPickLineItemId ?? 0
                    });
                }
            });

            _lastPickLineItem = _pick.PickLineItems.Last();
        }

        private async void Submit(UserPicks picks)
        {
            var requestObject = new UserPicksRequestObject();

            picks.Season = _games.Season;
            picks.Week = _games.Week;

            requestObject.UserPick = picks;
            HttpResponseMessage response;

            if (!UserPickId.HasValue)
                response = await Client.PostAsJsonAsync("api/user-pick", requestObject);
            else
                response = await Client.PutAsJsonAsync($"/api/user-pick", requestObject);

            if (response.IsSuccessStatusCode)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Success,
                    Summary = $"Week {picks.Week} picks successfully submitted",
                    Duration = 4000
                });
            }
            else
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "Error submitting picks",
                    Detail = "Please try again.",
                    Duration = 4000
                });
            }
        }

        private void Cancel()
        {
            NavigationManager.NavigateTo("/");
        }

#pragma warning restore CS8602 // Dereference of a possibly null reference.

#pragma warning restore CS8601 // Possible null reference assignment.
    }
}