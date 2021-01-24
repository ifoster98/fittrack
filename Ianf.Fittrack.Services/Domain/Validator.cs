using System;
using System.Collections.Generic;
using System.Linq;
using Ianf.Fittrack.Services.Dto;
using Ianf.Fittrack.Services.Errors;
using LanguageExt;

namespace Ianf.Fittrack.Services.Domain
{
    public static class Validator
    {
        public static Either<IEnumerable<DtoValidationError>, ExerciseEntry> ValidateDto(this Dto.ExerciseEntry set)
        {
            var errors = new List<DtoValidationError>();
            var reps = new PositiveInt();
            PositiveInt.CreatePositiveInt(set.Reps)
                .Match(
                    None: () => errors.Add(new DtoValidationError("Invalid amount for reps.", "ExerciseEntry", "Reps") ),
                    Some: (s) => reps = s
                );
            var weight = new Weight();
            Weight.CreateWeight(set.Weight)
                .Match(
                    None: () => errors.Add(new DtoValidationError("Invalid amount for weight.", "ExerciseEntry", "Weight")),
                    Some: (s) => weight = s
                );
            var order = new PositiveInt();
            PositiveInt.CreatePositiveInt(set.Order)
                .Match(
                    None: () => errors.Add(new DtoValidationError("Invalid amount for order.", "ExerciseEntry", "Order")),
                    Some: (s) => order = s
                );
            if(errors.Any()) return errors;
            return new ExerciseEntry(reps, weight, order);
        }

        public static Either<IEnumerable<DtoValidationError>, Exercise> ValidateDto(this Dto.Exercise exercise)
        {
            var errors = new List<DtoValidationError>();
            var sets = new List<ExerciseEntry>();

            exercise.ExerciseEntries.ForEach(s => {
                var e = ValidateDto(s);
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

        public static Either<IEnumerable<DtoValidationError>, PlannedWorkout> ValidateDto(this Dto.PlannedWorkout workout)
        {
            var errors = new List<DtoValidationError>();
            var exercises = new List<Exercise>();

            if (workout.Exercises == null)
            {
                errors.Add(new DtoValidationError("Exercises cannot be null.", "PlannedWorkout", "Exercises"));
            }
            else
            {
                workout.Exercises.ForEach(e =>
                {
                    var ex = ValidateDto(e);
                    ex.Match
                    (
                        Left: (err) => errors.AddRange(err),
                        Right: (r) => exercises.Add(r)
                    );
                });
            }

            var programName = new ProgramName();
            ProgramName.CreateProgramName(workout.ProgramName)
                .Match(
                    None: () => errors.Add(new DtoValidationError("Invalid program name.", "PlannedWorkout", "ProgramName")),
                    Some: (s) => programName = s
                );

            if(errors.Any()) return errors;
            return new PlannedWorkout(programName, workout.ProgramType, workout.WorkoutTime, exercises);
        }

        public static Either<IEnumerable<DtoValidationError>, ActualWorkout> ValidateDto(this Dto.ActualWorkout workout)
        {
            var errors = new List<DtoValidationError>();
            var exercises = new List<Exercise>();

            if (workout.Exercises == null)
            {
                errors.Add(new DtoValidationError("Exercises cannot be null.", "PlannedWorkout", "Exercises"));
            }
            else
            {
                workout.Exercises.ForEach(e =>
                {
                    var ex = ValidateDto(e);
                    ex.Match
                    (
                        Left: (err) => errors.AddRange(err),
                        Right: (r) => exercises.Add(r)
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
            return new ActualWorkout(programName, workout.ProgramType, workout.WorkoutTime, exercises);
        }
    }
}