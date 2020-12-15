using Ianf.Fittrack.Workouts.Domain;

namespace Ianf.Fittrack.Workouts.Persistance.Interfaces
{
    public interface IWorkoutRepository
    {
        PositiveInt SaveWorkout(Workout workout);
    }
}