using Microsoft.AspNetCore.Components;
using NFLWeeklyPicksUI.Models;

namespace NFLWeeklyPicksUI.Pages.PickTemplate
{
    public partial class PickRow
    {
        [Parameter] public WeeklyGameViewModel Game { get; set; } = new();

        [Parameter] public UserPickLineItems PickLine { get; set; } = new();

        [Parameter] public bool isLocked { get; set; }

        public int _currentlySelectedTeamId = 0;
        public bool HomeTeamSelected { get; set; }
        public bool AwayTeamSelected { get; set; }

        protected override void OnInitialized()
        {
            if (PickLine.UserPickLineItemId != 0)
            {
                _currentlySelectedTeamId = PickLine.PickTeamId;
                if (PickLine.PickTeamId == Game.HomeTeam.Id)
                {
                    HomeTeamSelected = true;
                    AwayTeamSelected = false;
                }
                else
                {
                    AwayTeamSelected = true;
                    HomeTeamSelected = false;
                }
            }
        }

        public void HandleCheckboxChange(bool value, string name)
        {
            _currentlySelectedTeamId = name == "awayTeam" ? Game.AwayTeam.Id : Game.HomeTeam.Id;

            if (name == "awayTeam")
            {
                AwayTeamSelected = value;
                HomeTeamSelected = !value;
            }
            else
            {
                HomeTeamSelected = value;
                AwayTeamSelected = !value;
            }

            PickLine.PickTeamId = _currentlySelectedTeamId;
        }
    }
}