using System.Collections.Generic;
using System.Linq;
using Ianf.Fittrack.Services.Errors;
using LanguageExt;

namespace Ianf.Fittrack.Services.Domain
{
    public static class Validator
    {
        public static Either<IEnumerable<DtoValidationError>, Set> ValidateDto(this Dto.Set set)
        {
            var errors = new List<DtoValidationError>();
            var plannedReps = new PositiveInt();
            PositiveInt.CreatePositiveInt(set.PlannedReps)
                .Match(
                    None: () => errors.Add(new DtoValidationError("Invalid amount for planned reps.", "Set", "PlannedReps") ),
                    Some: (s) => plannedReps = s
                );
            var plannedWeight = new Weight();
            Weight.CreateWeight(set.PlannedWeight)
                .Match(
                    None: () => errors.Add(new DtoValidationError("Invalid amount for planned weight.", "Set", "PlannedWeight")),
                    Some: (s) => plannedWeight = s
                );
            var actualReps = new NonNegativeInt();
            NonNegativeInt.CreateNonNegativeInt(set.ActualReps)
                .Match(
                    None: () => errors.Add(new DtoValidationError("Invalid amount for actual reps.", "Set", "ActualReps") ),
                    Some: (s) => actualReps = s
                );
            var actualWeight = new Weight();
            Weight.CreateWeight(set.ActualWeight)
                .Match(
                    None: () => errors.Add(new DtoValidationError("Invalid amount for actual weight.", "Set", "ActualWeight")),
                    Some: (s) => actualWeight = s
                );
            var order = new PositiveInt();
            PositiveInt.CreatePositiveInt(set.Order)
                .Match(
                    None: () => errors.Add(new DtoValidationError("Invalid amount for order.", "Set", "Order")),
                    Some: (s) => order = s
                );
            if(errors.Any()) return errors;
            return new Set(plannedReps, plannedWeight, actualReps, actualWeight, order);
        }

        public static Either<IEnumerable<DtoValidationError>, Exercise> ValidateDto(this Dto.Exercise exercise)
        {
            var errors = new List<DtoValidationError>();
            var sets = new List<Set>();

            exercise.Sets.ForEach(s => {
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

        public static Either<IEnumerable<DtoValidationError>, Workout> ValidateDto(this Dto.Workout workout)
        {
            var errors = new List<DtoValidationError>();
            var exercises = new List<Exercise>();

            if (workout.Exercises == null)
            {
                errors.Add(new DtoValidationError("Exercises cannot be null.", "Workout", "Exercises"));
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
            return new Workout(programName, workout.ProgramType, workout.WorkoutTime, exercises);
        }
    }
}