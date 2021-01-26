using Microsoft.AspNetCore.Mvc;
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

        [Route("/Workout")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<int> AddWorkout(Services.Dto.Workout workout)
        {
            var result = _workoutService.AddWorkout(workout);
            ActionResult<int> returnValue = Ok();
            result.Match(
                Left: (err) => returnValue = BadRequest(err),
                Right: (newWorkoutId) => returnValue = Ok(newWorkoutId)
            );

            return returnValue;
        }

        [HttpGet("/Workout/{workoutDay}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetNextWorkout(DateTime workoutDay)
        {
            var result = _workoutService.GetNextWorkout(workoutDay);
            IActionResult returnValue = Ok();
            result.Match(
                None: () => returnValue = NotFound(),
                Some: (nextWorkout) => returnValue = Ok(nextWorkout)
            );

            return returnValue;
        }
    }
}