using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using NFLWeeklyPicksUI.Models.Authentication;
using Radzen;
using System.Net.Http.Json;
using Microsoft.AspNetCore.WebUtilities;
using NFLWeeklyPicksUI.Services;

namespace NFLWeeklyPicksUI.Pages.Authentication
{
    public partial class LoginHome
    {
        [Inject] public HttpClient Client { get; set; }

        [Inject] public NavigationManager NavigationManager { get; set; }

        [Inject] public IAuthenticationService AuthenticationService { get; set; }

        [Inject] public NotificationService NotificationService { get; set; }

        private bool _showSuccess;
        private bool _showError;

        public async Task OnLogin(LoginArgs args)
        {
            var result = await AuthenticationService.Login(args.Username, args.Password);

            if (result.AccessToken == "2FARequired")
            {
                var queryParams = new Dictionary<string, string>()
                {
                    ["provider"] = "Email",
                    ["email"] = result.RefreshToken
                };

                NavigationManager.NavigateTo(QueryHelpers.AddQueryString("/two-step-verificaiton", queryParams));
                return;
            }


            if (string.IsNullOrEmpty(result.AccessToken))
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "Error logging in",
                    Detail = "Please try again.",
                    Duration = 4000
                });
                return;
            }

            NavigationManager.NavigateTo("");
        }

        private void OnRegister(string name)
        {
            NavigationManager.NavigateTo("register");
        }

        private async Task OnResetPassword(string value)
        {
            NavigationManager.NavigateTo("forgot-password");
        }
    }
}