using Microsoft.AspNetCore.Components;
using NFLWeeklyPicksUI.Models;
using System.Net.Http.Json;
using System.Security.Principal;
using Microsoft.AspNetCore.WebUtilities;
using NFLWeeklyPicksUI.Services;

namespace NFLWeeklyPicksUI.Pages.PickWeeks
{
    public partial class ListSeasonWeeks
    {
        [Inject] public HttpClient Client { get; set; }

        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public HttpInterceptorService Interceptor { get; set; }

        private IList<SeasonWeeksViewModel>? _seasonWeeks;
        private int _currentSeason = 2022; //TODO inject this

        protected override async Task OnInitializedAsync()
        {
            Interceptor.RegisterEvent();
            Interceptor.RegisterBeforeSendEvent();
            _seasonWeeks =
                await Client.GetFromJsonAsync<IList<SeasonWeeksViewModel>>($"api/season/{_currentSeason}/weeks");
        }

        private void OpenWeekPicks(SeasonWeeksViewModel viewModel)
        {
            if (viewModel.UserPickId > 0)
            {
                NavigationManager.NavigateTo($"/picks/{viewModel.UserPickId}");
            }
            else
            {
                NavigationManager.NavigateTo($"picks/{viewModel.Season}/{viewModel.WeekNumber}");
            }
        }

        private void OpenWeekPicksWithScore(SeasonWeeksViewModel viewModel)
        {
            NavigationManager.NavigateTo($"picks/scores/{viewModel.Season}/{viewModel.WeekNumber}");
        }

        public void Dispose() => Interceptor.DisposeEvent();
    }
}