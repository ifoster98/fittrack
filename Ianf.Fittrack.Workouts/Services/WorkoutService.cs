using System.Collections.Generic;
using System.Linq;
using Ianf.Fittrack.Workouts.Domain;
using Ianf.Fittrack.Workouts.Persistance.Interfaces;
using Ianf.Fittrack.Workouts.Services.Interfaces;
using LanguageExt;
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

        public static Either<IEnumerable<DtoValidationError>, Domain.Workout> ValidateWorkoutToAdd(Domain.Workout workout)
        {
            return workout;
        }
    }
}