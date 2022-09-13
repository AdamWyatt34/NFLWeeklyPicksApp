using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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
}