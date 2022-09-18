using MediatR;
using NFLWeeklyPicksAPI.Commands;
using Quartz;

namespace NFLWeeklyPicksAPI.Jobs;

public class WeeklyWinnerEmailJob : IJob
{
    private readonly IMediator _dispatcher;

    public WeeklyWinnerEmailJob(IMediator dispatcher)
    {
        _dispatcher = dispatcher;
    }


    public async Task Execute(IJobExecutionContext context)
    {
        await _dispatcher.Send(new EmailCalculatedRecords(), new CancellationToken());
    }
}