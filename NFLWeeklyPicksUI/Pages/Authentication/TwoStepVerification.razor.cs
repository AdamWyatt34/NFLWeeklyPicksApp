using Microsoft.AspNetCore.Components;
using NFLWeeklyPicksUI.Extensions;
using NFLWeeklyPicksUI.Services;

namespace NFLWeeklyPicksUI.Pages.Authentication;

public partial class TwoStepVerification
{
    [Inject] public IAuthenticationService AuthenticationService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }

    private string email;
    private string provider;
    private string code;
    private bool _showError;

    protected override void OnInitialized()
    {
        email = NavigationManager.ExtractQueryStringByKey<string>("email");
        provider = NavigationManager.ExtractQueryStringByKey<string>("provider");

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(provider))
            NavigationManager.NavigateTo("/");
    }

    private async Task Submit(string token)
    {
        _showError = false;

        var result = await AuthenticationService.TwoFactorVerification(email, provider, token);

        if (string.IsNullOrEmpty(result.AccessToken))
        {
            _showError = true;
            return;
        }

        NavigationManager.NavigateTo("/");
    }
}