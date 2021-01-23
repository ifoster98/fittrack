using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Ianf.Fittrack.Services.Interfaces;
using System.Collections.Generic;

namespace Ianf.Fittrack.Webapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MatchEventController : ControllerBase
    {
        private readonly IMatchEventService _matchEventService;

        public MatchEventController(IMatchEventService matchEventService)
        {
            _matchEventService = matchEventService;
        }

        [HttpGet("/LoginWithUserId/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> LoginWithUserId(int userId)
        {
            var result = await _matchEventService.LoginWithUserId(userId);
            return Ok(result);
        }

        [HttpGet("/Matches")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Services.Dto.Match>>> Matches()
        {
            var result = await _matchEventService.GetMatches();
            return Ok(result);
        }

        [HttpGet("/Events")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Services.Dto.Event>>> Events()
        {
            var result = await _matchEventService.GetEvents();
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> AddNewMatchEventAsync(Ianf.Fittrack.Services.Dto.MatchEvent matchEvent)
        {
            var result = await _matchEventService.AddNewMatchEventAsync(matchEvent);
            ActionResult<int> returnValue = Ok();
            return result.Match(
                Left: (err) => returnValue = BadRequest(err),
                Right: (newMatchEventId) => returnValue = Ok(newMatchEventId)
            );
        }

        [HttpGet("/MatchEvent/{userId}/{matchId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllMatchEventAsync(int userId, int matchId)
        {
            var result = await _matchEventService.GetAllMatchEventsByUserIdAsync(userId, matchId);
            IActionResult returnValue = Ok();
            result.Match(
                Left:(err) => returnValue = BadRequest(),
                Right:(ms) => returnValue = Ok(ms)
            );
            return returnValue;
        }
    }
}