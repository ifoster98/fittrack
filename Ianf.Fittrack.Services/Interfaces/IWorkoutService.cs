using LanguageExt;
using System.Collections.Generic;
using Ianf.Fittrack.Services.Errors;
using System;
using Ianf.Fittrack.Services.Domain;

namespace Ianf.Fittrack.Services.Interfaces
{
    public interface IWorkoutService
    {
        Either<IEnumerable<DtoValidationError>, PositiveInt> AddPlannedWorkout(Dto.PlannedWorkout workout);

        Option<Dto.PlannedWorkout> GetNextWorkout(DateTime workoutDay, string programName);

        Either<IEnumerable<DtoValidationError>, PositiveInt> AddActualWorkout(Dto.ActualWorkout workout);
    }
}
