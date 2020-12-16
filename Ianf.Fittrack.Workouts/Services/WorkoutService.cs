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

        public Either<IEnumerable<Error>, PositiveInt> AddNewWorkout(Dto.Workout workout) => 
            ToDomain(workout)
                .Bind(ValidateWorkoutToAdd)
                .Match<Either<IEnumerable<Error>, PositiveInt>>
                (
                    Left: (err) => Left(err),
                    Right: (w) => _workoutRepository.SaveWorkout(w)
                );

        public static Either<IEnumerable<Error>, Domain.Workout> ValidateWorkoutToAdd(Domain.Workout workout)
        {
            return workout;
        }
    }
}