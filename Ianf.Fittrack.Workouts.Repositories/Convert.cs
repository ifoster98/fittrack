using System;
using System.Linq;
using System.Reflection.Metadata;

namespace Ianf.Fittrack.Workouts.Repositories
{
    public static class Convert
    {
//        public static Either<IEnumerable<EntityValidationError>, Set> ToDomain(this Entity.Set set)
//        {
//            var errors = new List<EntityValidationError>();
//            var reps = new PositiveInt();
//            PositiveInt.CreatePositiveInt(set.Reps)
//                .Match(
//                    None: () => errors.Add(new EntityValidationError("Invalid amount for reps.", "Set", "Reps") ),
//                    Some: (s) => reps = s
//                );
//            var weight = new Weight();
//            Weight.CreateWeight(set.Weight)
//                .Match(
//                    None: () => errors.Add(new EntityValidationError("Invalid amount for weight.", "Set", "Weight")),
//                    Some: (s) => weight = s
//                );
//            var order = new PositiveInt();
//            PositiveInt.CreatePositiveInt(set.Order)
//                .Match(
//                    None: () => errors.Add(new EntityValidationError("Invalid amount for order.", "Set", "Order")),
//                    Some: (s) => order = s
//                );
//            if(errors.Any()) return errors;
//            return new Set(reps, weight, order);
//        }

        public static Entity.Set ToEntity(this Domain.Set set)  =>
            new Entity.Set() {
                Reps = set.Reps.Value,
                Weight = set.Weight.Value,
                Order = set.Order.Value
            };

//        public static Either<IEnumerable<EntityValidationError>, Exercise> ToDomain(this Entity.Exercise exercise)
//        {
//            var errors = new List<EntityValidationError>();
//            var sets = new List<Set>();
//
//            exercise.Sets.ForEach(s => {
//                var e = ToDomain(s);
//                e.Match
//                (
//                    Left: (err) => errors.AddRange(err),
//                    Right: (s) => sets.Add(s)
//                );
//            });
//
//            var order = new PositiveInt();
//            PositiveInt.CreatePositiveInt(exercise.Order)
//                .Match(
//                    None: () => errors.Add(new EntityValidationError("Invalid amount for order.", "Exercise", "Order")),
//                    Some: (s) => order = s
//                );
//            if(errors.Any()) return errors;
//            return new Exercise(exercise.ExerciseType, sets, order);
//        }

        public static Entity.Exercise ToEntity(this Domain.Exercise exercise) =>
            new Entity.Exercise() {
                ExerciseType = (byte)exercise.ExerciseType,
                Order = exercise.Order.Value,
                Sets = exercise.Sets.Select(s => s.ToEntity()).ToList()
            };

//        public static Either<IEnumerable<EntityValidationError>, Workout> ToDomain(this Entity.Workout workout)
//        {
//            var errors = new List<EntityValidationError>();
//            var plannedExercises = new List<Exercise>();
//            var actualExercises = new List<Exercise>();
//
//            if (workout.PlannedExercises == null)
//            {
//                errors.Add(new EntityValidationError("Planned exercises cannot be null.", "Workout", "PlannedExercises"));
//            }
//            else
//            {
//                workout.PlannedExercises.ForEach(e =>
//                {
//                    var ex = ToDomain(e);
//                    ex.Match
//                    (
//                        Left: (err) => errors.AddRange(err),
//                        Right: (r) => plannedExercises.Add(r)
//                    );
//                });
//            }
//
//            if (workout.ActualExercises == null)
//            {
//                errors.Add(new EntityValidationError("Actual exercises cannot be null.", "Workout", "ActualExercises"));
//            }
//            else
//            {
//                workout.ActualExercises.ForEach(e =>
//                {
//                    var ex = ToDomain(e);
//                    ex.Match(
//                        Left: (err) => errors.AddRange(err),
//                        Right: (r) => actualExercises.Add(r)
//                    );
//                });
//            }
//
//            var programName = new ProgramName();
//            ProgramName.CreateProgramName(workout.ProgramName)
//                .Match(
//                    None: () => errors.Add(new EntityValidationError("Invalid program name.", "Workout", "ProgramName")),
//                    Some: (s) => programName = s
//                );
//
//            if(errors.Any()) return errors;
//            return new Workout(programName, workout.WorkoutTime, plannedExercises, actualExercises);
//        }

        public static Entity.Workout ToEntity(this Domain.Workout workout) =>
            new Entity.Workout() {
                WorkoutTime = workout.WorkoutTime,
                ProgramName = workout.ProgramName.Value,
                PlannedExercises = workout.PlannedExercises.Select(p => p.ToEntity()).ToList()
            };
    }
}