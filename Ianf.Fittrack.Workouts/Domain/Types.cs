using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;

namespace Ianf.Fittrack.Workouts.Domain 
{
    public enum ExerciseType {
        BenchPress,
        Squat,
        Deadlift,
        OverheadPress,
        BentOverRow
    }

    public record Set(ExerciseType ExerciseType, PositiveInt Reps, Weight Weight, PositiveInt Order);

    public record Exercise(List<Set> Sets, PositiveInt Order);

    public record Workout(ProgramName programName, DateTime WorkoutTime, List<Exercise> PlannedExercises, List<Exercise> ActualExercises);

    public record Error(string errorMessage);

    public static class Convert
    {
        public static Either<IEnumerable<Error>, Set> ToDomain(Dto.Set set)
        {
            var errors = new List<Error>();
            var reps = new PositiveInt();
            PositiveInt.CreatePositiveInt(set.Reps)
                .Match(
                    None: () => errors.Add(new Error("Invalid amount for reps.")),
                    Some: (s) => reps = s
                );
            var weight = new Weight();
            Weight.CreateWeight(set.Weight)
                .Match(
                    None: () => errors.Add(new Error("Invalid amount for weight.")),
                    Some: (s) => weight = s
                );
            var order = new PositiveInt();
            PositiveInt.CreatePositiveInt(set.Order)
                .Match(
                    None: () => errors.Add(new Error("Invalid amount for order.")),
                    Some: (s) => order = s
                );
            if(errors.Any()) return errors;
            return new Set(set.ExerciseType, reps, weight, order);
        }

        public static Either<IEnumerable<Error>, Exercise> ToDomain(Dto.Exercise exercise)
        {
            var errors = new List<Error>();
            var sets = new List<Set>();

            exercise.Sets.ForEach(s => {
                var e = ToDomain(s);
                e.Match
                (
                    Left: (err) => errors.AddRange(err),
                    Right: (s) => sets.Add(s)
                );
            });

            var order = new PositiveInt();
            PositiveInt.CreatePositiveInt(exercise.Order)
                .Match(
                    None: () => errors.Add(new Error("Invalid amount for order.")),
                    Some: (s) => order = s
                );
            if(errors.Any()) return errors;
            return new Exercise(sets, order);
        }

        public static Either<IEnumerable<Error>, Workout> ToDomain(Dto.Workout workout)
        {
            var errors = new List<Error>();
            var plannedExercises = new List<Exercise>();
            var actualExercises = new List<Exercise>();

            workout.PlannedExercises.ForEach(e => {
                var ex = ToDomain(e);
                ex.Match
                (
                    Left: (err) => errors.AddRange(err),
                    Right: (r) => plannedExercises.Add(r)
                );
            });

            workout.ActualExercises.ForEach(e => {
                var ex = ToDomain(e);
                ex.Match(
                    Left: (err) => errors.AddRange(err),
                    Right: (r) => actualExercises.Add(r) 
                );
            });

            var programName = new ProgramName();
            ProgramName.CreateProgramName(workout.ProgramName)
                .Match(
                    None: () => errors.Add(new Error("Invalid program name.")),
                    Some: (s) => programName = s
                );

            if(errors.Any()) return errors;
            return new Workout(programName, workout.WorkoutTime, plannedExercises, actualExercises);
        }
    }
}