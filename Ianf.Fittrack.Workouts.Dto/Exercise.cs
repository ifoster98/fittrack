using System.Linq;
using System.Collections.Generic;

namespace Ianf.Fittrack.Workouts.Dto
{
    public struct Exercise 
    {
        public ExerciseType ExerciseType { get; set; }
        public List<Set> Sets { get; set; }
        public int Order { get; set; }
    }
}
