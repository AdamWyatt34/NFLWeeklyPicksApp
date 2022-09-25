using System.Net;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using NFLWeeklyPicksUI.AuthProviders;
using NFLWeeklyPicksUI.Models.Authentication;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;

namespace NFLWeeklyPicksUI.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _client;
        private readonly AuthenticationStateProvider _authenticationState;
        private readonly ILocalStorageService _localStorage;
        private readonly NavigationManager _navigationManager;

        public AuthenticationService(HttpClient client, AuthenticationStateProvider authenticationState,
            ILocalStorageService localStorage, NavigationManager navigationManager)
        {
            _client = client;
            _authenticationState = authenticationState;
            _localStorage = localStorage;
            _navigationManager = navigationManager;
        }

        public async Task<bool> ConfirmEmail(string token, string email)
        {
            var postBody = new { token, email };
            var confirmation = await _client.PostAsJsonAsync("api/authentication/confirm-email", postBody);

            return await confirmation.Content.ReadFromJsonAsync<bool>();
        }

        public async Task<LoginToken> Login(string username, string password)
        {
            var body = new { username, password };
            var login = await _client.PostAsJsonAsync("api/authentication/login", body);

            var token = await login.Content.ReadFromJsonAsync<LoginToken>();

            if (token?.AccessToken == "2FARequired")
                return token;

            await _localStorage.SetItemAsync("authToken", token.AccessToken);
            await _localStorage.SetItemAsync("refreshToken", token.RefreshToken);
            ((AuthStateProvider)_authenticationState).NotifyUserAuthentication(token.AccessToken);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token.AccessToken);

            return token;
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            await _localStorage.RemoveItemAsync("refreshToken");

            ((AuthStateProvider)_authenticationState).NotifyUserLogout();

            _client.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<string> RefreshToken()
        {
            var authToken = await _localStorage.GetItemAsync<string>("authToken");
            var refreshToken = await _localStorage.GetItemAsync<string>("refreshToken");

            var body = new { AccessToken = authToken, RefreshToken = refreshToken };

            var response = await _client.PostAsJsonAsync("api/token/refresh", new
            {
                Token = body
            });

            if (!response.IsSuccessStatusCode)
            {
                _navigationManager.NavigateTo("/logout");
                return null;
            }

            var token = await response.Content.ReadFromJsonAsync<LoginToken>();

            await _localStorage.SetItemAsync("authToken", token.AccessToken);
            await _localStorage.SetItemAsync("refreshToken", token.RefreshToken);
            ((AuthStateProvider)_authenticationState).NotifyUserAuthentication(token.AccessToken);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token.AccessToken);

            return token.AccessToken;
        }

        public async Task<HttpStatusCode> ForgotPassword(string emailAddress)
        {
            var result = await _client.PostAsJsonAsync("api/authentication/forgot-password",
                new { EmailAddress = emailAddress });

            return result.StatusCode;
        }

        public async Task<bool> ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            var result = await _client.PostAsJsonAsync("api/authentication/reset-password", resetPasswordModel);

            return await result.Content.ReadFromJsonAsync<bool>();
        }

        public async Task<LoginToken> TwoFactorVerification(string email, string provider, string twoFactorToken)
        {
            var body = new { Email = email, provider = provider, TwoFactorToken = twoFactorToken };
            var login = await _client.PostAsJsonAsync("api/authentication/verify-two-factor", body);

            var token = await login.Content.ReadFromJsonAsync<LoginToken>();

            await _localStorage.SetItemAsync("authToken", token.AccessToken);
            await _localStorage.SetItemAsync("refreshToken", token.RefreshToken);
            ((AuthStateProvider)_authenticationState).NotifyUserAuthentication(token.AccessToken);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token.AccessToken);

            return token;
        }


        public async Task<IdentityResult> RegisterUser(RegisterUserViewModel registerUser)
        {
            var body = new { UserForRegistration = registerUser };
            var register = await _client.PostAsJsonAsync("api/authentication", body);

            return await register.Content.ReadFromJsonAsync<IdentityResult>();
        }
    }
}