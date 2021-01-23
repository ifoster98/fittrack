using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Ianf.Fittrack.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;

namespace Ianf.Fittrack.Webapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorkoutController : ControllerBase
    {
        private readonly ILogger<WorkoutController> _logger;
        private readonly IWorkoutService _workoutService;

        public WorkoutController(ILogger<WorkoutController> logger, IWorkoutService workoutService)
        {
            _logger = logger;
            _workoutService = workoutService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> AddNewWorkoutAsync(Services.Dto.PlannedWorkout workout)
        {
            var result = await _workoutService.AddNewWorkoutAsync(workout);
            ActionResult<int> returnValue = Ok();
            result.Match(
                Left: (err) => returnValue = BadRequest(err),
                Right: (newWorkoutId) => returnValue = Ok(newWorkoutId)
            );

            return returnValue;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetNextWorkoutAsync(DateTime workoutDay, string programName)
        {
            var result = await _workoutService.GetNextWorkoutAsync(workoutDay, programName);
            IActionResult returnValue = Ok();
            result.Match(
                None: () => returnValue = NotFound(),
                Some: (nextWorkout) => returnValue = Ok(nextWorkout)
            );

            return returnValue;
        }
    }
}