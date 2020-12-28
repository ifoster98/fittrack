using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ianf.Fittrack.Workouts.Domain;
using LanguageExt;

namespace Ianf.Fittrack.Workouts.Persistance.Interfaces
{
    public interface IWorkoutRepository
    {
        Task<PositiveInt> SaveWorkoutAsync(PlannedWorkout workout);

        Task<PositiveInt> SaveWorkoutAsync(ActualWorkout workout);

        Task<List<PlannedWorkout>> GetWorkoutsAfterDate(DateTime workoutDate);

        Task<bool> HasWorkout(DateTime workoutDate, ProgramName programName);
    }
}