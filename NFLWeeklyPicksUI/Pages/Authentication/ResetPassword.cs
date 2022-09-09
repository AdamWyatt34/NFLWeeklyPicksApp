using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.WebUtilities;
using NFLWeeklyPicksUI.Extensions;
using NFLWeeklyPicksUI.Models.Authentication;
using NFLWeeklyPicksUI.Services;

namespace NFLWeeklyPicksUI.Pages.Authentication;

public partial class ResetPassword
{
    private readonly ResetPasswordModel _resetPasswordModel = new();
    private bool _showError;
    private bool _showSuccess;

    [Inject] public IAuthenticationService AuthenticationService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }

    protected override void OnInitialized()
    {
        var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
        var queryStrings = QueryHelpers.ParseQuery(uri.Query);

        _resetPasswordModel.Token = NavigationManager.ExtractQueryStringByKey<string>("token");
        _resetPasswordModel.Email = NavigationManager.ExtractQueryStringByKey<string>("email");

        if (string.IsNullOrWhiteSpace(_resetPasswordModel.Token) ||
            string.IsNullOrWhiteSpace(_resetPasswordModel.Email))
            NavigationManager.NavigateTo("/");
    }

    private async Task Submit(ResetPasswordModel args)
    {
        _showSuccess = _showError = false;
        var result = await AuthenticationService.ResetPassword(args);
        if (result)
            _showSuccess = true;
        else
            _showError = true;
    }
}