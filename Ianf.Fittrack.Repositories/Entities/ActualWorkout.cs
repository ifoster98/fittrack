using System;
using System.Collections.Generic;

#nullable disable

namespace Ianf.Fittrack.Repositories.Entities
{
    public partial class ActualWorkout
    {
        public ActualWorkout()
        {
            Exercises = new HashSet<Exercise>();
        }

        public int Id { get; set; }
        public int PlannedWorkoutId { get; set; }
        public string ProgramName { get; set; }
        public byte ProgramType { get; set; }
        public DateTime WorkoutTime { get; set; }

        public virtual PlannedWorkout PlannedWorkout { get; set; }
        public virtual ICollection<Exercise> Exercises { get; set; }
    }
}
