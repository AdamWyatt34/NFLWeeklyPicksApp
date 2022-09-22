using System.Net;
using Microsoft.AspNetCore.Components;
using NFLWeeklyPicksUI.Models.Authentication;
using NFLWeeklyPicksUI.Services;

namespace NFLWeeklyPicksUI.Pages.Authentication;

public partial class ForgotPassword
{
    private bool _showSuccess;
    private bool _showError;
    [Inject] public IAuthenticationService AuthService { get; set; }
    private ForgotPasswordModel _forgotPasswordModel = new();

    private async Task Submit(ForgotPasswordModel forgotPasswordModel)
    {
        _showSuccess = _showError = false;

        var result = await AuthService.ForgotPassword(forgotPasswordModel.Email);
        if (result == HttpStatusCode.OK)
            _showSuccess = true;
        else
            _showError = true;
    }
}