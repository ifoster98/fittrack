using System;
using System.Linq;
using System.Reflection.Metadata;
using Ianf.Fittrack.Workouts.Domain;

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

        public static Exercise ToDomain(this Entities.Exercise exercise) =>
            new Exercise(
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

        public static Workout ToDomain(this Entities.Workout workout) =>
            new Workout(
                workout.Id,
                ProgramName.CreateProgramName(workout.ProgramName).IfNone(new ProgramName()),
                workout.WorkoutTime,
                workout.Exercises.Select(e => ToDomain(e)).ToList()
            );

        public static Entities.Workout ToEntity(this Domain.Workout workout) =>
            new Entities.Workout() {
                WorkoutTime = workout.WorkoutTime,
                ProgramName = workout.ProgramName.Value,
                Exercises = workout.Exercises.Select(p => p.ToEntity()).ToList()
            };
    }
}