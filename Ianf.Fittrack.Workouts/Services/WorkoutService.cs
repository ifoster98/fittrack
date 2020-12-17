using Ianf.Fittrack.Workouts.Domain;
using Ianf.Fittrack.Workouts.Persistance.Interfaces;
using Ianf.Fittrack.Workouts.Services.Interfaces;
using LanguageExt;
using System.Collections.Generic;
using System.Linq;
using static Ianf.Fittrack.Workouts.Domain.Convert;
using static LanguageExt.Prelude;

namespace Ianf.Fittrack.Workouts.Services
{
    public class WorkoutService : IWorkoutService 
    {
        private readonly IWorkoutRepository _workoutRepository;

        public WorkoutService(IWorkoutRepository workoutRepository)
        {
            _workoutRepository = workoutRepository;
        }

        public Either<IEnumerable<DtoValidationError>, PositiveInt> AddNewWorkout(Dto.Workout workout) => 
            workout
                .ToDomain()
                .Bind(ValidateWorkoutToAdd)
                .Match<Either<IEnumerable<DtoValidationError>, PositiveInt>>
                (
                    Left: (err) => Left(err),
                    Right: (w) => _workoutRepository.SaveWorkout(w)
                );

        public static Either<IEnumerable<DtoValidationError>, Workout> ValidateWorkoutToAdd(Workout workout)
        {
            var errors = new List<DtoValidationError>();
            if (workout.ActualExercises.Any()) errors.Add(new DtoValidationError("Cannot have actual exercises mapped in a new workout.", "Workout", "ActualExercises"));
            if (workout.PlannedExercises.Count == 0) errors.Add(new DtoValidationError("Must have planned exercises mapped in a new workout.", "Workout", "PlannedExercises"));
            if (errors.Any()) return errors;
            return workout;
        }
    }
}