using NFLWeeklyPicksUI.Models;
using NFLWeeklyPicksUI.Pages.PickScores;

namespace NFLWeeklyPicksUI.Services;

public interface IUserPickService
{
    Task<List<UserPickWeeklyRecordViewModel>> GetUserRecords(int season, int week);
}