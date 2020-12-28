using System;
using System.Collections.Generic;

#nullable disable

namespace Ianf.Fittrack.Workouts.Repositories.Entities
{
    public partial class PlannedWorkout
    {
        public PlannedWorkout()
        {
            Exercises = new HashSet<Exercise>();
        }

        public int Id { get; set; }
        public string ProgramName { get; set; }
        public DateTime WorkoutTime { get; set; }

        public virtual ICollection<Exercise> Exercises { get; set; }
    }
}
