using System.Linq.Expressions;
using NFLWeeklyPicksAPI.Models.Entities;

namespace NFLWeeklyPicksAPI.ViewModels;

public class SeasonViewModel
{
    public int SeasonId { get; set; }
    public int Year { get; set; }

    public static Expression<Func<Season, SeasonViewModel>> Selector =>
        record => new SeasonViewModel()
        {
            SeasonId = record.SeasonId,
            Year = record.Year
        };
}