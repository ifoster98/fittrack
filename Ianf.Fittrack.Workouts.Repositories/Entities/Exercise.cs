using System;
using System.Collections.Generic;

#nullable disable

namespace Ianf.Fittrack.Workouts.Repositories.Entities
{
    public partial class Exercise
    {
        public Exercise()
        {
            Sets = new HashSet<Set>();
        }

        public int Id { get; set; }
        public int? PlannedWorkoutId { get; set; }
        public int? ActualWorkoutId { get; set; }
        public byte ExerciseType { get; set; }
        public int Order { get; set; }

        public virtual PlannedWorkout PlannedWorkout { get; set; }
        public virtual ActualWorkout ActualWorkout { get; set; }
        public virtual ICollection<Set> Sets { get; set; }
    }
}
