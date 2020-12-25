using Ianf.Fittrack.Workouts.Domain;
using Ianf.Fittrack.Workouts.Persistance.Interfaces;
using Ianf.Fittrack.Workouts.Services.Interfaces;
using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Ianf.Fittrack.Workouts.Domain.Convert;

namespace Ianf.Fittrack.Workouts.Services
{
    public class WorkoutService : IWorkoutService 
    {
        private readonly IWorkoutRepository _workoutRepository;

        public WorkoutService(IWorkoutRepository workoutRepository) =>
            _workoutRepository = workoutRepository;

        public async Task<Either<IEnumerable<DtoValidationError>, PositiveInt>> AddNewWorkoutAsync(Dto.Workout workout) => 
            await workout
                .ToDomain()
                .BindAsync(ValidateWorkoutToAdd)
                .MapAsync(w => _workoutRepository.SaveWorkoutAsync(w));

        public static async Task<Either<IEnumerable<DtoValidationError>, Workout>> ValidateWorkoutToAdd(Workout workout)
        {
            var errors = new List<DtoValidationError>();
            if (workout.Exercises.Count == 0) errors.Add(new DtoValidationError("Must have exercises mapped in a new workout.", "Workout", "Exercises"));
            if (errors.Any()) return errors;
            return workout;
        }

        public async Task<Option<Dto.Workout>> GetNextWorkoutAsync(DateTime workoutDay) 
        {
            throw new NotImplementedException();
        }
    }
}