using System;
using System.Linq;
using System.Reflection.Metadata;
using Ianf.Fittrack.Workouts.Domain;
using Ianf.Fittrack.Workouts.Dto;

namespace Ianf.Fittrack.Workouts.Repositories
{
    public static class Convert
    {
        public static Domain.Set ToDomain(this Entities.Set set) =>
            new Domain.Set(
                PositiveInt.CreatePositiveInt(set.Reps).IfNone(new PositiveInt()),
                Weight.CreateWeight(set.Weight).IfNone(new Weight()),
                PositiveInt.CreatePositiveInt(set.Order).IfNone(new PositiveInt())
            );

        public static Entities.Set ToEntity(this Domain.Set set)  =>
            new Entities.Set() {
                Reps = set.Reps.Value,
                Weight = set.Weight.Value,
                Order = set.Order.Value
            };

        public static Domain.Exercise ToDomain(this Entities.Exercise exercise) =>
            new Domain.Exercise(
                (ExerciseType)exercise.ExerciseType,
                exercise.Sets.Select(s => ToDomain(s)).ToList(),
                PositiveInt.CreatePositiveInt(exercise.Order).IfNone(new PositiveInt())
            );

        public static Entities.Exercise ToEntity(this Domain.Exercise exercise) =>
            new Entities.Exercise() {
                ExerciseType = (byte)exercise.ExerciseType,
                Order = exercise.Order.Value,
                Sets = exercise.Sets.Select(s => s.ToEntity()).ToList()
            };

        public static Domain.PlannedWorkout ToDomain(this Entities.PlannedWorkout workout) =>
            new Domain.PlannedWorkout(
                workout.Id,
                ProgramName.CreateProgramName(workout.ProgramName).IfNone(new ProgramName()),
                workout.WorkoutTime,
                workout.Exercises.Select(e => ToDomain(e)).ToList()
            );

        public static Entities.PlannedWorkout ToEntity(this Domain.PlannedWorkout workout) =>
            new Entities.PlannedWorkout() {
                WorkoutTime = workout.WorkoutTime,
                ProgramName = workout.ProgramName.Value,
                Exercises = workout.Exercises.Select(p => p.ToEntity()).ToList()
            };

        public static Domain.ActualWorkout ToDomain(this Entities.ActualWorkout workout) =>
            new Domain.ActualWorkout(
                workout.Id,
                ToDomain(workout.PlannedWorkout),
                ProgramName.CreateProgramName(workout.ProgramName).IfNone(new ProgramName()),
                workout.WorkoutTime,
                workout.Exercises.Select(e => ToDomain(e)).ToList()
            );

        public static Entities.ActualWorkout ToEntity(this Domain.ActualWorkout workout) =>
            new Entities.ActualWorkout() {
                PlannedWorkout = ToEntity(workout.PlannedWorkout),
                WorkoutTime = workout.WorkoutTime,
                ProgramName = workout.ProgramName.Value,
                Exercises = workout.Exercises.Select(p => p.ToEntity()).ToList()
            };
    }
}