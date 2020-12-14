using Ianf.Fittrack.Dto;
using Ianf.Fittrack.Workouts.Persistance.Interfaces;
using Ianf.Fittrack.Workouts.Services.Interfaces;

namespace Ianf.Fittrack.Workouts.Services
{
    public class WorkoutService : IWorkoutService 
    {
        private readonly IWorkoutRepository _workoutRepository;

        public WorkoutService(IWorkoutRepository workoutRepository)
        {
            _workoutRepository = workoutRepository;
        }

        public void AddNewWorkout(Workout workout)
        {
            // Convert Dto to domain layer 

            // Validate workout internally

            // Save workout to persistance layer
        }
    }
}