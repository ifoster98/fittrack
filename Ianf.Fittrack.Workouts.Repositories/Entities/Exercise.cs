using System.Collections.Generic;

namespace Ianf.Fittrack.Workouts.Repositories.Entity
{
    public class Exercise
    {
        public int Id { get; set; }
        public byte ExerciseType { get; set; }
        public int Order { get; set; }
        public ICollection<Set> Sets { get; set; }
    }
}