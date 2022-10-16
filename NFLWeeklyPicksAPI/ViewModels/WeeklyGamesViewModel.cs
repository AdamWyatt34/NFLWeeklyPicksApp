using System.Linq.Expressions;
using NFLWeeklyPicksAPI.Models.Entities;

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
        public int CompetitionId { get; set; }
        public long EspnCompetitonId { get; set; }
        public string GameName { get; set; }
        public DateTime GameDate { get; set; }
        public TeamViewModel HomeTeam { get; set; }
        public TeamViewModel AwayTeam { get; set; }
        public string Odds { get; set; }

        internal static Expression<Func<Competitions, WeeklyGameViewModel>> Selector =>
            record => new WeeklyGameViewModel()
            {
                CompetitionId = record.CompetitionsId,
                EspnCompetitonId = record.EspnCompetitionId,
                GameName = record.GameName,
                GameDate = record.GameDate,
                Odds = record.Odds,
                HomeTeam = new TeamWithScoreViewModel()
                {
                    Id = record.HomeTeamId,
                    Record = record.HomeTeamScoreUrl
                },
                AwayTeam = new TeamWithScoreViewModel()
                {
                    Id = record.AwayTeamId,
                    Record = record.AwayTeamScoreUrl
                }
            };
    }

    public class WeeklyGameWithScoreViewModel : WeeklyGameViewModel
    {
        public TeamWithScoreViewModel HomeTeam { get; set; }
        public TeamWithScoreViewModel AwayTeam { get; set; }
        public List<UserPickScoreViewModel> UserPicks { get; set; }

        internal static Expression<Func<Competitions, WeeklyGameWithScoreViewModel>> Selector =>
            record => new WeeklyGameWithScoreViewModel()
            {
                CompetitionId = record.CompetitionsId,
                EspnCompetitonId = record.EspnCompetitionId,
                GameName = record.GameName,
                GameDate = record.GameDate,
                Odds = record.Odds,
                HomeTeam = new TeamWithScoreViewModel()
                {
                    Id = record.HomeTeamId,
                    Record = record.HomeTeamScoreUrl
                },
                AwayTeam = new TeamWithScoreViewModel()
                {
                    Id = record.AwayTeamId,
                    Record = record.AwayTeamScoreUrl
                }
            };
    }


    public class UserPickScoreViewModel
    {
        public string Username { get; set; }
        public int UserPickNumber { get; set; }
        public int SelectedTeamId { get; set; }
        public string SelectedTeam { get; set; }
        public string SelectedTeamAbbreviation { get; set; }
        public int CompetitionType { get; set; }
        public int CompetitionPoints { get; set; }
    }

    public class UserPickScoreViewModelWithWinner : UserPickScoreViewModel
    {
        public int UserPickId { get; set; }
        public bool IsCorrect { get; set; }
    }

    public class WeeklyGameWithScoreAndWinnerViewModel : WeeklyGameWithScoreViewModel
    {
        public int WinningTeamId => Math.Abs(HomeTeam.Score - AwayTeam.Score) == 0 ? 0 :
            HomeTeam.Score > AwayTeam.Score ? HomeTeam.Id : AwayTeam.Id;

        public List<UserPickScoreViewModelWithWinner> UserPicks { get; set; }
    }

    public class TeamViewModel
    {
        public int Id { get; set; }
        public int EspnTeamId { get; set; }
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