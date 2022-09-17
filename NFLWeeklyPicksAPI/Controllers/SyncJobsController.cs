using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NFLWeeklyPicksAPI.Commands;
using NFLWeeklyPicksAPI.Commands.Competitions;
using NFLWeeklyPicksAPI.Commands.SeasonWeeks;
using NFLWeeklyPicksAPI.Commands.Teams;

namespace NFLWeeklyPicksAPI.Controllers;

[Route("api/sync")]
[ApiController]
public class SyncJobsController : ControllerBase
{
    private readonly IMediator _dispatcher;

    public SyncJobsController(IMediator dispatcher)
    {
        _dispatcher = dispatcher;
    }

    [Route("teams"), HttpPost, ProducesResponseType((int)HttpStatusCode.NoContent)]
    public async Task<Unit> SyncTeamsWithEspn([FromQuery] SyncTeamsWithEspn query) => await _dispatcher.Send(query);

    [Route("season-weeks"), HttpPost, ProducesResponseType((int)HttpStatusCode.NoContent)]
    public async Task<Unit> SyncSeasonWeeks(SyncSeasonWeeks query) => await _dispatcher.Send(query);

    [Route("competitions"), HttpPost, ProducesResponseType((int)HttpStatusCode.NoContent)]
    public async Task<Unit> SyncCompetitionsWithEspn(SyncCompetitionsWithEspn query) => await _dispatcher.Send(query);

    [Route("record-email"), HttpPost, ProducesResponseType((int)HttpStatusCode.NoContent)]
    public async Task<Unit> EmailCalculatedRecords(EmailCalculatedRecords query) => await _dispatcher.Send(query);
}