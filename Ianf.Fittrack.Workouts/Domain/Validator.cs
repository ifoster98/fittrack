using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;

namespace Ianf.Fittrack.Workouts.Domain 
{
    public static class Validator
    {
        public static Either<IEnumerable<DtoValidationError>, Set> Validate(this Dto.Set set)
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

        public static Either<IEnumerable<DtoValidationError>, Exercise> Validate(this Dto.Exercise exercise)
        {
            var errors = new List<DtoValidationError>();
            var sets = new List<Set>();

            exercise.Sets.ForEach(s => {
                var e = Validate(s);
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

        public static Either<IEnumerable<DtoValidationError>, Workout> Validate(this Dto.Workout workout)
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
                    var ex = Validate(e);
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
            return new Workout(workout.Id, programName, workout.WorkoutTime, plannedExercises);
        }
    }
}