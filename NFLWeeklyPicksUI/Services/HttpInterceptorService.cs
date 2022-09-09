using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components;
using Toolbelt.Blazor;

namespace NFLWeeklyPicksUI.Services;

public class HttpInterceptorService
{
    private readonly HttpClientInterceptor _interceptor;
    private readonly RefreshTokenService _tokenService;
    private readonly NavigationManager _navigationManager;

    public HttpInterceptorService(HttpClientInterceptor interceptor, RefreshTokenService tokenService,
        NavigationManager navigationManager)
    {
        _interceptor = interceptor;
        _tokenService = tokenService;
        _navigationManager = navigationManager;
    }

    public void RegisterEvent() => _interceptor.AfterSend += HandleResponse;

    public void RegisterBeforeSendEvent() => _interceptor.BeforeSendAsync += InterceptBeforeSendAsync;

    private async Task InterceptBeforeSendAsync(object sender,
        HttpClientInterceptorEventArgs e)
    {
        var absolutePath = e.Request.RequestUri.AbsolutePath;

        if (!absolutePath.Contains("token") && !absolutePath.Contains("account"))
        {
            var token = await _tokenService.TryRefreshToken();
            if (!string.IsNullOrEmpty(token))
            {
                e.Request.Headers.Authorization =
                    new AuthenticationHeaderValue("bearer", token);
            }
        }
    }

    private void HandleResponse(object sender, HttpClientInterceptorEventArgs e)
    {
        if (e.Response == null)
        {
            _navigationManager.NavigateTo("/error");
            throw new HttpResponseException("Server not available.");
        }

        var message = "";

        if (!e.Response.IsSuccessStatusCode)
        {
            switch (e.Response.StatusCode)
            {
                case HttpStatusCode.NotFound:
                    _navigationManager.NavigateTo("/404");
                    message = "Resource not found.";
                    break;
                // case HttpStatusCode.BadRequest:
                //     message = "Invalid request. Please try again.";
                //     _toastService.ShowError(message);
                //     break;
                case HttpStatusCode.Unauthorized:
                    _navigationManager.NavigateTo("/unauthorized");
                    message = "Unauthorized access";
                    break;
                default:
                    _navigationManager.NavigateTo("/error");
                    message = "Something went wrong. Please contact the administrator.";
                    break;
            }

            throw new HttpResponseException(message);
        }
    }

    public void DisposeEvent()
    {
        _interceptor.AfterSend -= HandleResponse;
        _interceptor.BeforeSendAsync -= InterceptBeforeSendAsync;
    }
}