using NFLWeeklyPicksUI.Models;
using NFLWeeklyPicksUI.Pages.PickScores;

namespace NFLWeeklyPicksUI.Services;

public interface IUserPickService
{
    Task<List<UserPickWeeklyRecordViewModel>> GetUserRecords(int season, int week);

    Task<List<UnpaidPickViewModel>> GetUnpaidPicks(int season, int week);

    Task<bool> MarkUnpaidPicks(IEnumerable<UnpaidPickViewModel> picks);
}