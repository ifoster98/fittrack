using System;
using System.Collections.Generic;
using System.Linq;
using Ianf.Fittrack.Services.Dto;
using LanguageExt;

namespace Ianf.Fittrack.Services.Domain 
{
    public record PlannedWorkout(ProgramName ProgramName, ProgramType ProgramType, DateTime WorkoutTime, List<Exercise> Exercises) 
    { 
        public Dto.PlannedWorkout ToDto() =>
            new Dto.PlannedWorkout() {
                WorkoutTime = this.WorkoutTime,
                ProgramName = this.ProgramName.Value,
                ProgramType = this.ProgramType,
                Exercises = this.Exercises.Select(p => p.ToDto()).ToList()
            };
    }
}