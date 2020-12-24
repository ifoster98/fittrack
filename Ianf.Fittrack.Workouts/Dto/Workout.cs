using System;
using System.Collections.Generic;

namespace Ianf.Fittrack.Dto
{
    public struct Workout 
    {
        public string ProgramName { get; set; }
        public DateTime WorkoutTime { get; set; }
        public List<Exercise> PlannedExercises { get; set; }
    }
}