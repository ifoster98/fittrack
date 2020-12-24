using Ianf.Fittrack.Workouts.Domain;
using Ianf.Fittrack.Workouts.Persistance.Interfaces;
using Ianf.Fittrack.Workouts.Services.Interfaces;
using LanguageExt;
using System.Collections.Generic;
using System.Linq;
using static Ianf.Fittrack.Workouts.Domain.Convert;

namespace Ianf.Fittrack.Workouts.Services
{
    public class WorkoutService : IWorkoutService 
    {
        private readonly IWorkoutRepository _workoutRepository;

        public WorkoutService(IWorkoutRepository workoutRepository) =>
            _workoutRepository = workoutRepository;

        public Either<IEnumerable<DtoValidationError>, PositiveInt> AddNewWorkout(Dto.Workout workout) => 
            workout
                .ToDomain()
                .Bind(ValidateWorkoutToAdd)
                .Map(_workoutRepository.SaveWorkout);

        public static Either<IEnumerable<DtoValidationError>, Workout> ValidateWorkoutToAdd(Workout workout)
        {
            var errors = new List<DtoValidationError>();
            if (workout.PlannedExercises.Count == 0) errors.Add(new DtoValidationError("Must have planned exercises mapped in a new workout.", "Workout", "PlannedExercises"));
            if (errors.Any()) return errors;
            return workout;
        }

        public Option<Dto.Workout> GetNextWorkout() =>
            _workoutRepository.GetNextWorkout().Map(s => s.ToDto());
    }
}