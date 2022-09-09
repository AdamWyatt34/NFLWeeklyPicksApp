using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using NFLWeeklyPicksUI.Extensions;
using NFLWeeklyPicksUI.Services;

namespace NFLWeeklyPicksUI.Pages.Authentication
{
    public partial class ConfirmEmail
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IAuthenticationService AuthenticationService { get; set; }

        private string Token { get; set; }

        private string Email { get; set; }

        private bool _isLoading = true;

        private bool _showError = false;

        protected override async Task OnInitializedAsync()
        {
            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);

            Token = NavigationManager.ExtractQueryStringByKey<string>("token");
            Email = NavigationManager.ExtractQueryStringByKey<string>("email");

            var result = await AuthenticationService.ConfirmEmail(Token, Email);

            if (!result)
            {
                _showError = true;
                _isLoading = false;
                return;
            }

            _isLoading = false;
        }
    }
}