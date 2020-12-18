using Ianf.Fittrack.Workouts.Domain;
using Ianf.Fittrack.Workouts.Persistance.Interfaces;

namespace Ianf.Fittrack.Workouts.Persistance
{
    public class workoutRepository : IWorkoutRepository
    {
        public PositiveInt SaveWorkout(Workout workout) =>
            PositiveInt.CreatePositiveInt(1).IfNone(new PositiveInt());
    }
}