using System.Collections.Generic;
using Ianf.Fittrack.Workouts.Domain;
using Ianf.Fittrack.Workouts.Persistance.Interfaces;
using Ianf.Fittrack.Workouts.Services.Interfaces;
using LanguageExt;

namespace Ianf.Fittrack.Workouts.Services
{
    public class WorkoutService : IWorkoutService 
    {
        private readonly IWorkoutRepository _workoutRepository;

        public WorkoutService(IWorkoutRepository workoutRepository)
        {
            _workoutRepository = workoutRepository;
        }

        public Either<IEnumerable<Error>, PositiveInt> AddNewWorkout(Dto.Workout workout)
        {
            // Convert Dto to domain layer 

            // Validate workout internally

            // Save workout to persistance layer
            _workoutRepository.SaveWorkout(workout);
        }
    }
}