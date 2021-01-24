using System;
using System.Collections.Generic;
using System.Linq;
using Ianf.Fittrack.Services.Dto;
using LanguageExt;

namespace Ianf.Fittrack.Services.Domain
{
    public record ActualWorkout(ProgramName ProgramName, ProgramType ProgramType, DateTime WorkoutTime, List<Exercise> Exercises) 
    { 
        public Dto.ActualWorkout ToDto() =>
            new Dto.ActualWorkout() {
                WorkoutTime = this.WorkoutTime,
                ProgramName = this.ProgramName.Value,
                ProgramType = this.ProgramType,
                Exercises = this.Exercises.Select(p => p.ToDto()).ToList()
            };
    }
}