using System;
using System.Linq;
using System.Reflection.Metadata;
using Ianf.Fittrack.Workouts.Domain;

namespace Ianf.Fittrack.Workouts.Repositories
{
    public static class Convert
    {
        public static Domain.Set ToDomain(this Entity.Set set) =>
            new Domain.Set(
                PositiveInt.CreatePositiveInt(set.Reps).IfNone(new PositiveInt()),
                Weight.CreateWeight(set.Weight).IfNone(new Weight()),
                PositiveInt.CreatePositiveInt(set.Order).IfNone(new PositiveInt())
            );

        public static Entity.Set ToEntity(this Domain.Set set)  =>
            new Entity.Set() {
                Reps = set.Reps.Value,
                Weight = set.Weight.Value,
                Order = set.Order.Value
            };

        public static Exercise ToDomain(this Entity.Exercise exercise) =>
            new Exercise(
                (ExerciseType)exercise.ExerciseType,
                exercise.Sets.Select(s => ToDomain(s)).ToList(),
                PositiveInt.CreatePositiveInt(exercise.Order).IfNone(new PositiveInt())
            );

        public static Entity.Exercise ToEntity(this Domain.Exercise exercise) =>
            new Entity.Exercise() {
                ExerciseType = (byte)exercise.ExerciseType,
                Order = exercise.Order.Value,
                Sets = exercise.Sets.Select(s => s.ToEntity()).ToList()
            };

        public static Workout ToDomain(this Entity.Workout workout) =>
            new Workout(
                ProgramName.CreateProgramName(workout.ProgramName).IfNone(new ProgramName()),
                workout.WorkoutTime,
                workout.PlannedExercises.Select(e => ToDomain(e)).ToList()
            );

        public static Entity.Workout ToEntity(this Domain.Workout workout) =>
            new Entity.Workout() {
                WorkoutTime = workout.WorkoutTime,
                ProgramName = workout.ProgramName.Value,
                PlannedExercises = workout.PlannedExercises.Select(p => p.ToEntity()).ToList()
            };
    }
}