using System.Collections.Generic;
using Ianf.Fittrack.Workouts.Domain;
using LanguageExt;

namespace Ianf.Fittrack.Workouts.Services.Interfaces
{
    public interface IWorkoutService
    {
        Either<IEnumerable<DtoValidationError>, PositiveInt> AddNewWorkout(Dto.Workout workout);
    }
}