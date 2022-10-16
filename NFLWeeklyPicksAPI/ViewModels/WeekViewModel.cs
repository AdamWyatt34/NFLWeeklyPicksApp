using System.Linq.Expressions;

namespace NFLWeeklyPicksAPI.ViewModels;

public class WeekViewModel
{
    public int SeasonWeekId { get; set; }
    public string WeekDescription { get; set; }

    public static Expression<Func<Models.Entities.SeasonWeeks, WeekViewModel>> Selector =>
        record => new WeekViewModel
        {
            SeasonWeekId = record.SeasonWeeksId,
            WeekDescription = record.WeekDescription
        };
}