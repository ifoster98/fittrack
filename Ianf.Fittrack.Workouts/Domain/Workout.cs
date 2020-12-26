using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;

namespace Ianf.Fittrack.Workouts.Domain 
{
    public record Workout(int Id, ProgramName ProgramName, DateTime WorkoutTime, List<Exercise> Exercises) 
    { 
        public Dto.Workout ToDto() =>
            new Dto.Workout() {
                Id = this.Id,
                WorkoutTime = this.WorkoutTime,
                ProgramName = this.ProgramName.Value,
                Exercises = this.Exercises.Select(p => p.ToDto()).ToList()
            };
    }
}