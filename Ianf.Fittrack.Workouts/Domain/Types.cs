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

    public record Set(PositiveInt Reps, Weight Weight, PositiveInt Order) { };

    public record Exercise(ExerciseType ExerciseType, List<Set> Sets, PositiveInt Order) { };

    public record Workout(ProgramName ProgramName, DateTime WorkoutTime, List<Exercise> Exercises) { };

    public record Error(string ErrorMessage) { };

    public record DtoValidationError(string errorMessage, string DtoType, string DtoProperty) : Error(errorMessage) { };

    public static class Convert
    {
        public static Either<IEnumerable<DtoValidationError>, Set> ToDomain(this Dto.Set set)
        {
            var errors = new List<DtoValidationError>();
            var reps = new PositiveInt();
            PositiveInt.CreatePositiveInt(set.Reps)
                .Match(
                    None: () => errors.Add(new DtoValidationError("Invalid amount for reps.", "Set", "Reps") ),
                    Some: (s) => reps = s
                );
            var weight = new Weight();
            Weight.CreateWeight(set.Weight)
                .Match(
                    None: () => errors.Add(new DtoValidationError("Invalid amount for weight.", "Set", "Weight")),
                    Some: (s) => weight = s
                );
            var order = new PositiveInt();
            PositiveInt.CreatePositiveInt(set.Order)
                .Match(
                    None: () => errors.Add(new DtoValidationError("Invalid amount for order.", "Set", "Order")),
                    Some: (s) => order = s
                );
            if(errors.Any()) return errors;
            return new Set(reps, weight, order);
        }

        public static Dto.Set ToDto(this Set set)  =>
            new Dto.Set() {
                Reps = set.Reps.Value,
                Weight = set.Weight.Value,
                Order = set.Order.Value
            };

        public static Either<IEnumerable<DtoValidationError>, Exercise> ToDomain(this Dto.Exercise exercise)
        {
            var errors = new List<DtoValidationError>();
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
                    None: () => errors.Add(new DtoValidationError("Invalid amount for order.", "Exercise", "Order")),
                    Some: (s) => order = s
                );
            if(errors.Any()) return errors;
            return new Exercise(exercise.ExerciseType, sets, order);
        }

        public static Dto.Exercise ToDto(this Exercise exercise) =>
            new Dto.Exercise() {
                ExerciseType = exercise.ExerciseType,
                Order = exercise.Order.Value,
                Sets = exercise.Sets.Select(s => s.ToDto()).ToList()
            };

        public static Either<IEnumerable<DtoValidationError>, Workout> ToDomain(this Dto.Workout workout)
        {
            var errors = new List<DtoValidationError>();
            var plannedExercises = new List<Exercise>();
            var actualExercises = new List<Exercise>();

            if (workout.Exercises == null)
            {
                errors.Add(new DtoValidationError(" exercises cannot be null.", "Workout", "Exercises"));
            }
            else
            {
                workout.Exercises.ForEach(e =>
                {
                    var ex = ToDomain(e);
                    ex.Match
                    (
                        Left: (err) => errors.AddRange(err),
                        Right: (r) => plannedExercises.Add(r)
                    );
                });
            }

            var programName = new ProgramName();
            ProgramName.CreateProgramName(workout.ProgramName)
                .Match(
                    None: () => errors.Add(new DtoValidationError("Invalid program name.", "Workout", "ProgramName")),
                    Some: (s) => programName = s
                );

            if(errors.Any()) return errors;
            return new Workout(programName, workout.WorkoutTime, plannedExercises);
        }

        public static Dto.Workout ToDto(this Workout workout) =>
            new Dto.Workout() {
                WorkoutTime = workout.WorkoutTime,
                ProgramName = workout.ProgramName.Value,
                Exercises = workout.Exercises.Select(p => p.ToDto()).ToList()
            };
    }
}