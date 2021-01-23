using System.Threading.Tasks;
using LanguageExt;
using System.Collections.Generic;
using Ianf.Fittrack.Services.Errors;
using System;
using Ianf.Fittrack.Services.Domain;

namespace Ianf.Fittrack.Services.Interfaces
{
    public interface IWorkoutService
    {
        Task<Either<IEnumerable<DtoValidationError>, PositiveInt>> AddNewWorkoutAsync(Dto.PlannedWorkout workout);

        Task<Option<Dto.PlannedWorkout>> GetNextWorkoutAsync(DateTime workoutDay, string programName);
    }
}
