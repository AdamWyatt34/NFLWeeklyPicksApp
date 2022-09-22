using Microsoft.AspNetCore.Components;
using NFLWeeklyPicksUI.Models;
using NFLWeeklyPicksUI.Services;

namespace NFLWeeklyPicksUI.Pages.PickWeeks
{
    public partial class ListSeasonWeeks
    {
        [Inject] public ISeasonWeekService SeasonWeekService { get; set; }

        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public HttpInterceptorService Interceptor { get; set; }

        private List<SeasonWeeksViewModel> _seasonWeeks = new();
        private int _currentSeason = 2022; //TODO inject this

        protected override async Task OnInitializedAsync()
        {
            Interceptor.RegisterEvent();
            Interceptor.RegisterBeforeSendEvent();
            _seasonWeeks = (await SeasonWeekService.ListSeasonWeeks(_currentSeason)).ToList();
        }

        public void Dispose() => Interceptor.DisposeEvent();
    }
}