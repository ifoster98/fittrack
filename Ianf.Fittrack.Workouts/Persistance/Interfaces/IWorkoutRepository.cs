using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ianf.Fittrack.Workouts.Domain;
using LanguageExt;

namespace Ianf.Fittrack.Workouts.Persistance.Interfaces
{
    public interface IWorkoutRepository
    {
        Task<PositiveInt> SaveWorkoutAsync(Workout workout);

        Task<Option<Workout>> GetNextWorkoutAsync(DateTime workoutDay);

        Task<List<Workout>> GetWorkoutsAfterDate(DateTime workoutDate);
    }
}