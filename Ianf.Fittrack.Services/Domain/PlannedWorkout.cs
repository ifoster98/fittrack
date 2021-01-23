using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;

namespace Ianf.Fittrack.Services.Domain 
{
    public record PlannedWorkout(int Id, ProgramName ProgramName, DateTime WorkoutTime, List<Exercise> Exercises) 
    { 
        public Dto.PlannedWorkout ToDto() =>
            new Dto.PlannedWorkout() {
                Id = this.Id,
                WorkoutTime = this.WorkoutTime,
                ProgramName = this.ProgramName.Value,
                Exercises = this.Exercises.Select(p => p.ToDto()).ToList()
            };
    }
}