using System.Collections.Generic;

#nullable disable

namespace Ianf.Fittrack.Repositories.Entities
{
    public partial class Exercise
    {
        public Exercise()
        {
            ExerciseEntries = new HashSet<ExerciseEntry>();
        }

        public int Id { get; set; }
        public int? PlannedWorkoutId { get; set; }
        public int? ActualWorkoutId { get; set; }
        public byte ExerciseType { get; set; }
        public int Order { get; set; }

        public virtual ActualWorkout ActualWorkout { get; set; }
        public virtual PlannedWorkout PlannedWorkout { get; set; }
        public virtual ICollection<ExerciseEntry> ExerciseEntries { get; set; }
    }
}
