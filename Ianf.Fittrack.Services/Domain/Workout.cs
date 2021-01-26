using System;
using System.Collections.Generic;
using System.Linq;
using Ianf.Fittrack.Services.Dto;
using LanguageExt;

namespace Ianf.Fittrack.Services.Domain 
{
    public record Workout(ProgramName ProgramName, ProgramType ProgramType, DateTime WorkoutTime, List<Exercise> Exercises) 
    { 
        public Dto.Workout ToDto() =>
            new Dto.Workout() {
                WorkoutTime = this.WorkoutTime,
                ProgramName = this.ProgramName.Value,
                ProgramType = this.ProgramType,
                Exercises = this.Exercises.Select(p => p.ToDto()).ToList()
            };
    }
}