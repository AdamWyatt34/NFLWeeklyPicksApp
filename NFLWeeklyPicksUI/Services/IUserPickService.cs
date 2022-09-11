using NFLWeeklyPicksUI.Models;
using NFLWeeklyPicksUI.Pages.PickScores;

namespace NFLWeeklyPicksUI.Services;

public interface IUserPickService
{
    Task<WeeklyGamesWithScoreViewModel> GetPicksWithScores(int season, int week);
    Task<List<UserPickWeeklyRecordViewModel>> GetUserRecords(int season, int week);
}