using NFLWeeklyPicksUI.Models;

namespace NFLWeeklyPicksUI.Services;

public interface ISeasonWeekService
{
    Task<IEnumerable<SeasonWeeksViewModel>> ListSeasonWeeks(int season);
    Task<WeeklyGamesViewModel> ListWeeklyGames(int season, int week);
    Task<WeeklyGamesWithScoreViewModel> ListWeeklyGamesWithScore(int season, int week);
}