using System;
using System.Collections.Generic;

#nullable disable

namespace Ianf.Fittrack.Repositories.Entities
{
    public partial class PlannedWorkout
    {
        public PlannedWorkout()
        {
            ActualWorkouts = new HashSet<ActualWorkout>();
            Exercises = new HashSet<Exercise>();
        }

        public int Id { get; set; }
        public string ProgramName { get; set; }
        public byte ProgramType { get; set; }
        public DateTime WorkoutTime { get; set; }

        public virtual ICollection<ActualWorkout> ActualWorkouts { get; set; }
        public virtual ICollection<Exercise> Exercises { get; set; }
    }
}
