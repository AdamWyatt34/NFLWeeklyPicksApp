using System.Linq.Expressions;

namespace NFLWeeklyPicksAPI.ViewModels
{
    public class SeasonWeeksViewModel
    {
        public int SeasonWeeksId { get; set; }
        public int WeekNumber { get; set; }
        public string WeekDescription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsDisabled { get; set; }
        public IList<SeasonWeekUserPickViewModel> UserPicks { get; set; } = new List<SeasonWeekUserPickViewModel>();
        public int Season { get; set; }

        internal static Expression<Func<Models.Entities.SeasonWeeks, SeasonWeeksViewModel>> Selector =>
            record => new SeasonWeeksViewModel
            {
                SeasonWeeksId = record.SeasonWeeksId,
                WeekNumber = record.WeekNumber,
                WeekDescription = record.WeekDescription,
                StartDate = record.StartDate,
                EndDate = record.EndDate,
                IsDisabled = !(DateTime.Now < record.EndDate),
                Season = record.Season.Year
            };
    }

    public class SeasonWeekUserPickViewModel
    {
        public int UserPickId { get; set; }
        public string UserPickDescription { get; set; }
    }
}