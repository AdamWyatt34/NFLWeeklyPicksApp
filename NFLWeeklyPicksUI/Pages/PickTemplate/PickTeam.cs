using Microsoft.AspNetCore.Components;
using NFLWeeklyPicksUI.Models;

namespace NFLWeeklyPicksUI.Pages.PickTemplate
{
    public partial class PickTeam
    {
        [Parameter]
        public TeamViewModel Team { get; set; } = new();
        private bool _renderImage;

        public void OnChange(bool matches)
        {
            _renderImage = !matches;
        }
    }
}
