using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;

namespace Ianf.Fittrack.Services.Domain
{
    public record ActualWorkout(int Id, PlannedWorkout PlannedWorkout, ProgramName ProgramName, DateTime WorkoutTime, List<Exercise> Exercises) 
    { 
        public Dto.ActualWorkout ToDto() =>
            new Dto.ActualWorkout() {
                Id = this.Id,
                PlannedWorkout = this.PlannedWorkout.ToDto(),
                WorkoutTime = this.WorkoutTime,
                ProgramName = this.ProgramName.Value,
                Exercises = this.Exercises.Select(p => p.ToDto()).ToList()
            };
    }
}