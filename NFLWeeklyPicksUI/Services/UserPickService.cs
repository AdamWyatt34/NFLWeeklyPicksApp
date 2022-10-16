using System.Net.Http.Json;
using NFLWeeklyPicksUI.Models;
using NFLWeeklyPicksUI.Pages.PickScores;

namespace NFLWeeklyPicksUI.Services;

public class UserPickService : IUserPickService
{
    private readonly HttpClient _client;

    public UserPickService(HttpClient client)
    {
        _client = client;
    }

    public async Task<List<UserPickWeeklyRecordViewModel>> GetUserRecords(int season, int week)
    {
        var result =
            await _client.GetFromJsonAsync<List<UserPickWeeklyRecordViewModel>>($"api/user-pick/{season}/{week}");

        return result;
    }

    public async Task<List<UnpaidPickViewModel>> GetUnpaidPicks(int season, int week)
    {
        var result = await _client.GetFromJsonAsync<List<UnpaidPickViewModel>>($"api/user-pick/unpaid/{season}/{week}");

        return result;
    }

    public async Task<bool> MarkUnpaidPicks(IEnumerable<UnpaidPickViewModel> picks)
    {
        var result = await _client.PutAsJsonAsync($"api/user-pick/mark-unpaid", new { Picks = picks });

        return result.IsSuccessStatusCode;
    }
}