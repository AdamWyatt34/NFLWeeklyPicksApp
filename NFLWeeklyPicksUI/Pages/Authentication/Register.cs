using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using NFLWeeklyPicksUI.Models.Authentication;
using NFLWeeklyPicksUI.Services;

namespace NFLWeeklyPicksUI.Pages.Authentication
{
    public partial class Register
    {
        private RegisterUserViewModel _registerUser = new();

        [Inject] public IAuthenticationService AuthenticationService { get; set; }

        private bool _hasErrors = false;
        private bool _showSuccess = false;
        private bool _isBusy = false;
        private IEnumerable<RegisterErrorViewModel>? Errors { get; set; }

        private async Task Submit(RegisterUserViewModel registerUser)
        {
            _isBusy = true;
            var result = await AuthenticationService.RegisterUser(registerUser);

            if (result.Errors.Any())
            {
                Errors = result.Errors;
                _hasErrors = true;
            }
            else
            {
                _showSuccess = true;
            }

            _isBusy = false;
            StateHasChanged();
        }
    }
}