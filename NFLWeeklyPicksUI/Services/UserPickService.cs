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

    public async Task<WeeklyGamesWithScoreViewModel> GetPicksWithScores(int season, int week)
    {
        var result =
            await _client.GetFromJsonAsync<WeeklyGamesWithScoreViewModel>(
                $"api/NFL/weekly-scores?Season={season}&Week={week}");

        return result;
    }
}