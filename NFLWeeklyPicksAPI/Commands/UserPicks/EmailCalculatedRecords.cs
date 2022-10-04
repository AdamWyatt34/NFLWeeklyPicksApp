using System.Text;
using EmailService;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NFLWeeklyPicksAPI.Models.Entities;
using NFLWeeklyPicksAPI.Queries.NFL;
using NFLWeeklyPicksAPI.Queries.UserPicks;
using NFLWeeklyPicksAPI.ViewModels;

namespace NFLWeeklyPicksAPI.Commands;

public class EmailCalculatedRecords : IRequest<Unit>
{
    public class Handler : IRequestHandler<EmailCalculatedRecords, Unit>
    {
        private readonly IMediator _dispatcher;
        private readonly ApplicationDbContext _db;
        private readonly IEmailSender _emailSender;

        public Handler(IMediator dispatcher, ApplicationDbContext db, IEmailSender emailSender)
        {
            _dispatcher = dispatcher;
            _db = db;
            _emailSender = emailSender;
        }

        public async Task<Unit> Handle(EmailCalculatedRecords request, CancellationToken cancellationToken)
        {
            //Get calculated records
            var lastSeasonWeek =
                await _db.SeasonWeeks
                    .Include(sw => sw.Season)
                    .OrderByDescending(sw => sw.EndDate)
                    .FirstAsync(cancellationToken);

            var userRecords = await _dispatcher.Send(new CalculatePickRecordsWeek()
            {
                Season = lastSeasonWeek.Season.Year,
                WeekNumber = lastSeasonWeek.WeekNumber
            }, cancellationToken);

            var allWeeklyPicks = await _db.UserPicks
                .Include(up => up.PickLineItems)
                .ThenInclude(pli => pli.Competition)
                .Where(up => up.PickLineItems.Any(pli => pli.PickTypeId == (int)PickType.WithPoints))
                .Where(up => up.Season == lastSeasonWeek.Season.Year)
                .Where(up => up.Week == lastSeasonWeek.WeekNumber)
                .ToListAsync(cancellationToken);

            var lastCompetition = allWeeklyPicks.First()
                .PickLineItems.First(c => c.PickTypeId == (int)PickType.WithPoints).CompetitionId;

            var competition =
                await _db.Competitions.FirstAsync(c => c.CompetitionsId == lastCompetition, cancellationToken);

            var homeTeamEspnId = await _db.Teams
                .Where(t => t.TeamsId == competition.HomeTeamId)
                .Select(t => t.EspnTeamId)
                .FirstAsync(cancellationToken);

            var awayTeamEspnId = await _db.Teams
                .Where(t => t.TeamsId == competition.AwayTeamId)
                .Select(t => t.EspnTeamId)
                .FirstAsync(cancellationToken);

            var homeTeamScore = await _dispatcher.Send(new GetTeamScore()
            {
                CompetitionId = (int)competition.EspnCompetitionId,
                TeamId = homeTeamEspnId
            }, cancellationToken);
            var awayTeamScore = await _dispatcher.Send(new GetTeamScore()
            {
                CompetitionId = (int)competition.EspnCompetitionId,
                TeamId = awayTeamEspnId
            }, cancellationToken);
            var totalPoints = homeTeamScore + awayTeamScore;

            var finalCalculations = (from userRecord in userRecords.OrderByDescending(ur => ur.Wins)
                let pointsSubmitted = allWeeklyPicks.First(w => w.UserPicksId == userRecord.UserPickId)
                    .PickLineItems.First(pl => pl.CompetitionId == lastCompetition)
                    .PickPoints
                select (userRecord, Math.Abs(totalPoints - pointsSubmitted))).ToList();

            var orderedCalculations = finalCalculations
                .OrderByDescending(fc => int.Parse(fc.userRecord.Wins))
                .ThenBy(fc => fc.Item2);

            //Check if there are multiple entries with same point difference
            var smallestPointDifference = orderedCalculations.First().Item2;
            var mostWins = orderedCalculations.First().userRecord.Wins;

            var countWithSame = orderedCalculations.Count(oc =>
                oc.userRecord.Wins == mostWins && oc.Item2 == smallestPointDifference);

            var winningUsers = new List<string>();
            if (countWithSame == 1)
            {
                winningUsers.Add(orderedCalculations.First().userRecord.UserPickDescription);
            }
            else
            {
                var allWinners = orderedCalculations.Where(oc => oc.userRecord.Wins == mostWins)
                    .Where(oc => oc.Item2 == smallestPointDifference);

                winningUsers.AddRange(
                    allWinners.Select(winner => winner.userRecord.UserPickDescription));
            }

            var userIds = allWeeklyPicks.Select(al => al.UserId.ToString()).Distinct();

            var userEmails = await _db.Users
                .Where(u => userIds.Contains(u.Id))
                .Select(u => u.NormalizedEmail)
                .ToListAsync(cancellationToken);

            var message = new Message(userEmails,
                $"NFL Weekly Picks {lastSeasonWeek.WeekNumber} Results",
                GenerateBody(orderedCalculations, winningUsers), null);


            await _emailSender.SendEmailAsync(message);

            return Unit.Value;
        }

        private string GenerateBody(IEnumerable<(UserPickWeeklyRecordViewModel, double)> results,
            IEnumerable<string> winningUsers)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("Congratulations to: ").Append(string.Join(',', winningUsers))
                .Append(" for winning this week!<br/><br/>").AppendLine().AppendLine();

            stringBuilder.Append("Below is everyone's results for this week.<br/><br/>").AppendLine().AppendLine();
            foreach (var result in results)
            {
                stringBuilder.Append(
                    $"{result.Item1.UserPickDescription} - {result.Item1.Wins} Wins - {result.Item1.Losses} Losses - {result.Item2} point difference <br/> </br/> <br/>");
                stringBuilder.AppendLine().AppendLine();
            }

            stringBuilder.Append("Thank you!");
            return stringBuilder.ToString();
        }
    }
}