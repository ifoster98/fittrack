using System;
using System.Collections.Generic;

#nullable disable

namespace Ianf.Fittrack.Workouts.Repositories.Entities
{
    public partial class Set
    {
        public int Id { get; set; }
        public int ExerciseId { get; set; }
        public int Reps { get; set; }
        public decimal Weight { get; set; }
        public int Order { get; set; }

        public virtual Exercise Exercise { get; set; }
    }
}
