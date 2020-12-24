using System;
using System.Collections.Generic;

namespace Ianf.Fittrack.Workouts.Repositories.Entities
{
    public class Workout
    {
        public int Id { get; set; }
        public string ProgramName { get; set; }
        public DateTime WorkoutTime { get; set; }
        public ICollection<Exercise> PlannedExercises { get; set; }
        public ICollection<Exercise> ActualExercises { get; set; }
    }
}