using System.Linq;
using Ianf.Fittrack.Services.Domain;
using Ianf.Fittrack.Services.Dto;

namespace Ianf.Fittrack.Repositories
{
    public static class Convert
    {
        public static Services.Domain.Set ToDomain(this Entities.Set set) =>
            new Services.Domain.Set(
                PositiveInt.CreatePositiveInt(set.Reps).IfNone(new PositiveInt()),
                Weight.CreateWeight(set.Weight).IfNone(new Weight()),
                PositiveInt.CreatePositiveInt(set.Order).IfNone(new PositiveInt())
            );

        public static Entities.Set ToEntity(this Services.Domain.Set set)  =>
            new Entities.Set() {
                Reps = set.Reps.Value,
                Weight = set.Weight.Value,
                Order = set.Order.Value
            };

        public static Services.Domain.Exercise ToDomain(this Entities.Exercise exercise) =>
            new Services.Domain.Exercise(
                (ExerciseType)exercise.ExerciseType,
                exercise.Sets.Select(s => ToDomain(s)).ToList(),
                PositiveInt.CreatePositiveInt(exercise.Order).IfNone(new PositiveInt())
            );

        public static Entities.Exercise ToEntity(this Services.Domain.Exercise exercise) =>
            new Entities.Exercise() {
                ExerciseType = (byte)exercise.ExerciseType,
                Order = exercise.Order.Value,
                Sets = exercise.Sets.Select(s => s.ToEntity()).ToList()
            };

        public static Services.Domain.PlannedWorkout ToDomain(this Entities.PlannedWorkout workout) =>
            new Services.Domain.PlannedWorkout(
                workout.Id,
                ProgramName.CreateProgramName(workout.ProgramName).IfNone(new ProgramName()),
                workout.WorkoutTime,
                workout.Exercises.Select(e => ToDomain(e)).ToList()
            );

        public static Entities.PlannedWorkout ToEntity(this Services.Domain.PlannedWorkout workout) =>
            new Entities.PlannedWorkout() {
                WorkoutTime = workout.WorkoutTime,
                ProgramName = workout.ProgramName.Value,
                Exercises = workout.Exercises.Select(p => p.ToEntity()).ToList()
            };

        public static Services.Domain.ActualWorkout ToDomain(this Entities.ActualWorkout workout) =>
            new Services.Domain.ActualWorkout(
                workout.Id,
                ToDomain(workout.PlannedWorkout),
                ProgramName.CreateProgramName(workout.ProgramName).IfNone(new ProgramName()),
                workout.WorkoutTime,
                workout.Exercises.Select(e => ToDomain(e)).ToList()
            );

        public static Entities.ActualWorkout ToEntity(this Services.Domain.ActualWorkout workout) =>
                    new Entities.ActualWorkout() {
                PlannedWorkout = ToEntity(workout.PlannedWorkout),
                WorkoutTime = workout.WorkoutTime,
                ProgramName = workout.ProgramName.Value,
                Exercises = workout.Exercises.Select(p => p.ToEntity()).ToList()
            };
    }
}