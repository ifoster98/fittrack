using System;
using System.Collections.Generic;

namespace Ianf.Fittrack.Workouts.Dto
{
    public struct PlannedWorkout 
    {
        public int Id { get; set; }
        public string ProgramName { get; set; }
        public DateTime WorkoutTime { get; set; }
        public List<Exercise> Exercises { get; set; }

    }
}
