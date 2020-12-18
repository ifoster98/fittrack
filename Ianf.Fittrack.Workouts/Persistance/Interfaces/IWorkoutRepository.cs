using Ianf.Fittrack.Workouts.Domain;
using LanguageExt;

namespace Ianf.Fittrack.Workouts.Persistance.Interfaces
{
    public interface IWorkoutRepository
    {
        PositiveInt SaveWorkout(Workout workout);

        Option<Workout> GetNextWorkout();
    }
}