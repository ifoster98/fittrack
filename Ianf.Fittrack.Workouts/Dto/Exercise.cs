using System.Linq;
using System.Collections.Generic;
using Ianf.Fittrack.Workouts.Domain;

namespace Ianf.Fittrack.Dto
{
    public struct Exercise 
    {
        public ExerciseType ExerciseType { get; set; }
        public List<Set> Sets { get; set; }
        public int Order { get; set; }

        public override bool Equals(object? obj)
        {
            if(obj is null) return false;
            var item = (Exercise)obj;
            return item.ExerciseType.Equals(ExerciseType)
                && item.Order.Equals(Order)
                && item.Sets.SequenceEqual(Sets);
        }

        public override int GetHashCode()
        {
            throw new System.NotImplementedException();
        }
    }
}