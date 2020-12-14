using System;
using System.Collections.Generic;

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
}