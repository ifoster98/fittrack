using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using static LanguageExt.Prelude;
using Ianf.Fittrack.Services.Interfaces;
using Ianf.Fittrack.Services.Errors;
using Ianf.Fittrack.Services.Domain;

namespace Ianf.Fittrack.Services
{
    public class WorkoutService : IWorkoutService 
    {
        private readonly IWorkoutRepository _workoutRepository;

        public WorkoutService(IWorkoutRepository workoutRepository) 
        {
            _workoutRepository = workoutRepository;
        }

        public Either<IEnumerable<DtoValidationError>, PositiveInt> AddWorkout(Dto.Workout workout) => 
            workout
                .ValidateDto()
                .Bind(ValidateWorkoutToAdd)
                .Map(w => _workoutRepository.AddWorkout(w));

        public Either<IEnumerable<DtoValidationError>, Domain.Workout> ValidateWorkoutToAdd(Domain.Workout workout)
        {
            var errors = new List<DtoValidationError>();
            if (workout.Exercises.Count == 0) errors.Add(new DtoValidationError("Must have exercises mapped in a new workout.", "Workout", "Exercises"));
            var foo = _workoutRepository.HasWorkout(workout.WorkoutTime, workout.ProgramType, workout.ProgramName);
            if(foo) errors.Add(new DtoValidationError("Duplicate workout definition.", "Workout", ""));
            if (errors.Any()) return errors;
            return workout;
        }

        public Option<Dto.Workout> GetWorkoutForDate(DateTime workoutDay) 
        {
            if(workoutDay == DateTime.MinValue || workoutDay == DateTime.MaxValue) return None;
            var workouts = _workoutRepository.GetWorkoutsOnOrAfterDate(workoutDay.ToUniversalTime().Date);
            return workouts.Any()
                ? Some(workouts
                    .OrderBy(w => w.WorkoutTime)
                    .First()
                    .ToDto())
                : None;
        }
    }
}