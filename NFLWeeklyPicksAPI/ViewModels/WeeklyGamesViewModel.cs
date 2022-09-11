namespace NFLWeeklyPicksAPI.ViewModels
{
    public class WeeklyGamesViewModel
    {
        public int Season { get; set; }
        public int Week { get; set; }

        public List<WeeklyGameViewModel> Games { get; set; }
    }

    public class WeeklyGamesWithScoreViewModel
    {
        public int Season { get; set; }
        public int Week { get; set; }

        public List<WeeklyGameWithScoreViewModel> Games { get; set; }
    }

    public class WeeklyGameViewModel
    {
        public long CompetitionId { get; set; }
        public string GameName { get; set; }
        public DateTime GameDate { get; set; }
        public TeamViewModel HomeTeam { get; set; }
        public TeamViewModel AwayTeam { get; set; }
        public string Odds { get; set; }
    }

    public class WeeklyGameWithScoreViewModel : WeeklyGameViewModel
    {
        public TeamWithScoreViewModel HomeTeam { get; set; }
        public TeamWithScoreViewModel AwayTeam { get; set; }
        public List<UserPickScoreViewModel> UserPicks { get; set; }
    }

    public class UserPickScoreViewModel
    {
        public string Username { get; set; }
        public int UserPickNumber { get; set; }
        public int SelectedTeamId { get; set; }
        public string SelectedTeam { get; set; }
        public string SelectedTeamAbbreviation { get; set; }
    }

    public class UserPickScoreViewModelWithWinner : UserPickScoreViewModel
    {
        public bool IsCorrect { get; set; }
    }

    public class WeeklyGameWithScoreAndWinnerViewModel : WeeklyGameWithScoreViewModel
    {
        public int WinningTeamId => HomeTeam.Score > AwayTeam.Score ? HomeTeam.Id : AwayTeam.Id;
        public List<UserPickScoreViewModelWithWinner> UserPicks { get; set; }
    }

    public class TeamViewModel
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public string Nickname { get; set; }
        public string FullName { get; set; }
        public string LogoURL { get; set; }
        public string Record { get; set; }
        public string Abbreviation { get; set; }
    }

    public class TeamWithScoreViewModel : TeamViewModel
    {
        public double Score { get; set; }
    }
}