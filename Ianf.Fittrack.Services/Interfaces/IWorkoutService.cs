using LanguageExt;
using System.Collections.Generic;
using Ianf.Fittrack.Services.Errors;
using System;
using Ianf.Fittrack.Services.Domain;

namespace Ianf.Fittrack.Services.Interfaces
{
    public interface IWorkoutService
    {
        Either<IEnumerable<DtoValidationError>, PositiveInt> AddWorkout(Dto.Workout workout);

        Option<Dto.Workout> GetNextWorkout(DateTime workoutDay);
    }
}
