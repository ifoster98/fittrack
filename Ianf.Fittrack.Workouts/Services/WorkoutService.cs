using Ianf.Fittrack.Workouts.Domain;
using Ianf.Fittrack.Workouts.Persistance.Interfaces;
using Ianf.Fittrack.Workouts.Services.Interfaces;
using LanguageExt;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Ianf.Fittrack.Workouts.Domain.Validator;
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

        public async Task<Either<IEnumerable<DtoValidationError>, PositiveInt>> AddNewWorkoutAsync(Dto.Workout workout) => 
            await workout
                .ValidateDto()
                .BindAsync(ValidateWorkoutToAdd)
                .MapAsync(w => _workoutRepository.SaveWorkoutAsync(w));

        public async Task<Either<IEnumerable<DtoValidationError>, Workout>> ValidateWorkoutToAdd(Workout workout)
        {
            var errors = new List<DtoValidationError>();
            if (workout.Exercises.Count == 0) errors.Add(new DtoValidationError("Must have exercises mapped in a new workout.", "Workout", "Exercises"));
            var foo = await _workoutRepository.HasWorkout(workout.WorkoutTime, workout.ProgramName);
            if(foo) errors.Add(new DtoValidationError("Duplicate workout definition.", "Workout", ""));
            if (errors.Any()) return errors;
            return workout;
        }

        public async Task<Option<Dto.Workout>> GetNextWorkoutAsync(DateTime workoutDay, string programName) 
        {
            if(workoutDay == DateTime.MinValue || workoutDay == DateTime.MaxValue) return None;
            var workouts = await _workoutRepository.GetWorkoutsAfterDate(workoutDay);
            workouts = workouts.Where(w => w.ProgramName.Value.Equals(programName)).ToList();
            return workouts.Any()
                ? Some(workouts
                    .OrderBy(w => w.WorkoutTime)
                    .First()
                    .ToDto())
                : None;
        }
    }
}