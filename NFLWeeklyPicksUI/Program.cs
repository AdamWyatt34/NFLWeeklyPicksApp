using System.ComponentModel;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using NFLWeeklyPicksUI;
using NFLWeeklyPicksUI.AuthProviders;
using NFLWeeklyPicksUI.Options;
using NFLWeeklyPicksUI.Services;
using Radzen;
using Toolbelt.Blazor.Extensions.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var config = builder.Configuration;
var apiSettings = config.GetSection("ApiSettings").Get<ApiOptions>();

Uri uri = new(apiSettings.Url);

builder.Services.AddHttpClient("PicksAPI", (sp, cl) =>
{
    cl.BaseAddress = uri;
    cl.EnableIntercept(sp);
});
builder.Services.AddScoped(sp => sp.GetService<IHttpClientFactory>()?.CreateClient("PicksAPI"));
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IUserPickService, UserPickService>();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();
builder.Services.AddScoped<RefreshTokenService>();
builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddHttpClientInterceptor();
builder.Services.AddScoped<HttpInterceptorService>();
await builder.Build().RunAsync();