﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using NFLWeeklyPicksAPI.Queries.SeasonWeeks;
using NFLWeeklyPicksAPI.ViewModels;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace NFLWeeklyPicksAPI.Controllers
{
    [Route("api/season")]
    [ApiController]
    [Authorize]
    public class SeasonController
    {
        private readonly IMediator _dispatcher;

        public SeasonController(IMediator dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [Route("{Season}/weeks"), HttpGet, ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IEnumerable<SeasonWeeksViewModel>> ListSeasonWeeks([FromRoute] ListSeasonWeeks query) =>
            await _dispatcher.Send(query);
    }
}