using System.Linq;
using Ianf.Fittrack.Services.Domain;
using Ianf.Fittrack.Services.Dto;

namespace Ianf.Fittrack.Repositories
{
    public static class Convert
    {
        public static Services.Domain.ExerciseEntry ToDomain(this Entities.ExerciseEntry set) =>
            new Services.Domain.ExerciseEntry(
                PositiveInt.CreatePositiveInt(set.Reps).IfNone(new PositiveInt()),
                Weight.CreateWeight(set.Weight).IfNone(new Weight()),
                PositiveInt.CreatePositiveInt(set.Order).IfNone(new PositiveInt())
            );

        public static Entities.ExerciseEntry ToEntity(this Services.Domain.ExerciseEntry set)  =>
            new Entities.ExerciseEntry() {
                Reps = set.Reps.Value,
                Weight = set.Weight.Value,
                Order = set.Order.Value
            };

        public static Services.Domain.Exercise ToDomain(this Entities.Exercise exercise) =>
            new Services.Domain.Exercise(
                (ExerciseType)exercise.ExerciseType,
                exercise.ExerciseEntries.Select(s => ToDomain(s)).ToList(),
                PositiveInt.CreatePositiveInt(exercise.Order).IfNone(new PositiveInt())
            );

        public static Entities.Exercise ToEntity(this Services.Domain.Exercise exercise) =>
            new Entities.Exercise() {
                ExerciseType = (byte)exercise.ExerciseType,
                Order = exercise.Order.Value,
                ExerciseEntries = exercise.ExerciseEntries.Select(s => s.ToEntity()).ToList()
            };

        public static Services.Domain.PlannedWorkout ToDomain(this Entities.PlannedWorkout workout) =>
            new Services.Domain.PlannedWorkout(
                ProgramName.CreateProgramName(workout.ProgramName).IfNone(new ProgramName()),
                (ProgramType)workout.ProgramType,
                workout.WorkoutTime,
                workout.Exercises.Select(e => ToDomain(e)).ToList()
            );

        public static Entities.PlannedWorkout ToEntity(this Services.Domain.PlannedWorkout workout) =>
            new Entities.PlannedWorkout() {
                WorkoutTime = workout.WorkoutTime,
                ProgramName = workout.ProgramName.Value,
                ProgramType = (byte)workout.ProgramType,
                Exercises = workout.Exercises.Select(p => p.ToEntity()).ToList()
            };

        public static Services.Domain.ActualWorkout ToDomain(this Entities.ActualWorkout workout) =>
            new Services.Domain.ActualWorkout(
                ProgramName.CreateProgramName(workout.ProgramName).IfNone(new ProgramName()),
                (ProgramType)workout.ProgramType,
                workout.WorkoutTime,
                workout.Exercises.Select(e => ToDomain(e)).ToList()
            );

        public static Entities.ActualWorkout ToEntity(this Services.Domain.ActualWorkout workout) =>
            new Entities.ActualWorkout() {
                WorkoutTime = workout.WorkoutTime,
                ProgramName = workout.ProgramName.Value,
                ProgramType = (byte)workout.ProgramType,
                Exercises = workout.Exercises.Select(p => p.ToEntity()).ToList()
            };
    }
}