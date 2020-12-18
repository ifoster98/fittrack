using System.Collections.Generic;
using Ianf.Fittrack.Workouts.Domain;

namespace Ianf.Fittrack.Dto
{
    public struct Exercise 
    {
        public ExerciseType ExerciseType { get; set; }
        public List<Set> Sets { get; set; }
        public int Order { get; set; }
    }
}