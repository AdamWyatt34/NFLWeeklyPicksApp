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

        private List<SeasonWeeksViewModel> _seasonWeeks = new();
        private int _currentSeason = 2022; //TODO inject this

        protected override async Task OnInitializedAsync()
        {
            Interceptor.RegisterEvent();
            Interceptor.RegisterBeforeSendEvent();
            _seasonWeeks =
                await Client.GetFromJsonAsync<List<SeasonWeeksViewModel>>($"api/season/{_currentSeason}/weeks");
        }

        public void Dispose() => Interceptor.DisposeEvent();
    }
}