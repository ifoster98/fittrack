using System;
using System.Collections.Generic;
using Ianf.Fittrack.Workouts.Domain;
using Ianf.Fittrack.Workouts.Persistance.Interfaces;
using LanguageExt;

namespace Ianf.Fittrack.Workouts.Persistance
{
    public class workoutRepository : IWorkoutRepository
    {
        public PositiveInt SaveWorkout(Workout workout) =>
            PositiveInt.CreatePositiveInt(1).IfNone(new PositiveInt());

        public Option<Workout> GetNextWorkout()
        {
            var programName = ProgramName.CreateProgramName("Test Program").IfNone(new ProgramName());
            var workoutTime = DateTime.Now;
            var actualExercises = new List<Exercise>();
            var plannedExercises = new List<Exercise>();
            return new Workout(programName, workoutTime, plannedExercises, actualExercises);
        }
    }
}