using System.Net.Http.Json;
using NFLWeeklyPicksUI.Models;

namespace NFLWeeklyPicksUI.Services;

public class SeasonWeeksService : ISeasonWeekService
{
    private readonly HttpClient _client;

    public SeasonWeeksService(HttpClient client)
    {
        _client = client;
    }

    public async Task<IEnumerable<SeasonWeeksViewModel>> ListSeasonWeeks(int season)
    {
        var result = await _client.GetFromJsonAsync<IEnumerable<SeasonWeeksViewModel>>($"api/season/{season}/weeks");

        return result;
    }

    public async Task<WeeklyGamesViewModel> ListWeeklyGames(int season, int week)
    {
        var result = await _client.GetFromJsonAsync<WeeklyGamesViewModel>($"api/season/{season}/{week}");

        return result;
    }

    public async Task<WeeklyGamesWithScoreViewModel> ListWeeklyGamesWithScore(int season, int week)
    {
        var result =
            await _client.GetFromJsonAsync<WeeklyGamesWithScoreViewModel>(
                $"api/season/{season}/{week}/score");

        return result;
    }

    public async Task<IEnumerable<SeasonViewModel>> ListSeasons()
    {
        var result = await _client.GetFromJsonAsync<IEnumerable<SeasonViewModel>>($"api/season");

        return result;
    }

    public async Task<IEnumerable<WeekViewModel>> ListWeeks(int seasonId)
    {
        var result = await _client.GetFromJsonAsync<IEnumerable<WeekViewModel>>($"api/season/{seasonId}/week");

        return result;
    }
}