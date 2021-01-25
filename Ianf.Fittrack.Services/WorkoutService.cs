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

        public Either<IEnumerable<DtoValidationError>, PositiveInt> AddPlannedWorkout(Dto.PlannedWorkout workout) => 
            workout
                .ValidateDto()
                .Bind(ValidateWorkoutToAdd)
                .Map(w => _workoutRepository.AddWorkout(w));

        public Either<IEnumerable<DtoValidationError>, Domain.PlannedWorkout> ValidateWorkoutToAdd(Domain.PlannedWorkout workout)
        {
            var errors = new List<DtoValidationError>();
            if (workout.Exercises.Count == 0) errors.Add(new DtoValidationError("Must have exercises mapped in a new workout.", "PlannedWorkout", "Exercises"));
            var foo = _workoutRepository.HasWorkout(workout.WorkoutTime, workout.ProgramType, workout.ProgramName);
            if(foo) errors.Add(new DtoValidationError("Duplicate workout definition.", "PlannedWorkout", ""));
            if (errors.Any()) return errors;
            return workout;
        }

        public Option<Dto.PlannedWorkout> GetNextWorkout(DateTime workoutDay, string programName) 
        {
            if(workoutDay == DateTime.MinValue || workoutDay == DateTime.MaxValue) return None;
            var workouts = _workoutRepository.GetPlannedWorkoutsAfterDate(workoutDay);
            workouts = workouts.Where(w => w.ProgramName.Value.Equals(programName)).ToList();
            return workouts.Any()
                ? Some(workouts
                    .OrderBy(w => w.WorkoutTime)
                    .First()
                    .ToDto())
                : None;
        }

        public Either<IEnumerable<DtoValidationError>, PositiveInt> AddActualWorkout(Dto.ActualWorkout workout) => 
            workout
                .ValidateDto()
                .Bind(ValidateWorkoutToAdd)
                .Map(w => _workoutRepository.AddWorkout(w));

        public Either<IEnumerable<DtoValidationError>, Domain.ActualWorkout> ValidateWorkoutToAdd(Domain.ActualWorkout workout)
        {
            var errors = new List<DtoValidationError>();
            if (workout.Exercises.Count == 0) errors.Add(new DtoValidationError("Must have exercises mapped in a new workout.", "ActualWorkout", "Exercises"));
            var foo = _workoutRepository.HasWorkout(workout.WorkoutTime, workout.ProgramType, workout.ProgramName);
            if(foo) errors.Add(new DtoValidationError("Duplicate workout definition.", "ActualWorkout", ""));
            if (errors.Any()) return errors;
            return workout;
        }
    }
}