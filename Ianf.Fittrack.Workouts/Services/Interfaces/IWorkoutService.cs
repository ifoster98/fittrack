using System.Collections.Generic;
using System.Threading.Tasks;
using Ianf.Fittrack.Workouts.Domain;
using LanguageExt;

namespace Ianf.Fittrack.Workouts.Services.Interfaces
{
    public interface IWorkoutService
    {
        Task<Either<IEnumerable<DtoValidationError>, PositiveInt>> AddNewWorkoutAsync(Dto.Workout workout);

        Task<Option<Dto.Workout>> GetNextWorkoutAsync();
    }
}