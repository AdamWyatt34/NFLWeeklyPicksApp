using Microsoft.AspNetCore.Components;
using NFLWeeklyPicksUI.Models;

namespace NFLWeeklyPicksUI.Pages.PickTemplate
{
    public partial class PointEntry
    {
        [Parameter] public WeeklyGameViewModel LastGame { get; set; }

        [Parameter] public UserPickLineItems LastGamePickLineItem { get; set; }
        [Parameter] public bool isLocked { get; set; }

        private bool _fullNames;

        public void OnChange(bool matches)
        {
            _fullNames = !matches;
        }
    }
}