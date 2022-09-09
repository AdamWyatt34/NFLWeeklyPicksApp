using Microsoft.AspNetCore.Components;
using NFLWeeklyPicksUI.Services;

namespace NFLWeeklyPicksUI.Pages.Authentication
{
    public partial class Logout
    {
        [Inject]
        public IAuthenticationService Authentication { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await Authentication.Logout();
            NavigationManager.NavigateTo("/");
        }
    }
}