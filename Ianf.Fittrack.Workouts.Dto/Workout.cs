using System;
using System.Linq;
using System.Collections.Generic;

namespace Ianf.Fittrack.Workouts.Dto
{
    public struct Workout 
    {
        public int Id { get; set; }
        public string ProgramName { get; set; }
        public DateTime WorkoutTime { get; set; }
        public List<Exercise> Exercises { get; set; }

        public override bool Equals(object? obj)
        {
            if(obj is null) return false;
            var item = (Workout)obj;
            return item.ProgramName == ProgramName 
                && item.WorkoutTime == WorkoutTime
                && item.Exercises.SequenceEqual(Exercises);
        }

        public override int GetHashCode()
        {
            throw new System.NotImplementedException();
        }
    }
}