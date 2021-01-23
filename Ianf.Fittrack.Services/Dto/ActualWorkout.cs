using System;
using System.Collections.Generic;

namespace Ianf.Fittrack.Services.Dto
{
public struct ActualWorkout 
    {
        public int Id { get; set; }
        public PlannedWorkout PlannedWorkout { get; set; }
        public string ProgramName { get; set; }
        public DateTime WorkoutTime { get; set; }
        public List<Exercise> Exercises { get; set; }
    }
}
