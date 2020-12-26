using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;

namespace Ianf.Fittrack.Workouts.Domain 
{
    public record Exercise(ExerciseType ExerciseType, List<Set> Sets, PositiveInt Order) { 

        public Dto.Exercise ToDto() =>
            new Dto.Exercise() {
                ExerciseType = this.ExerciseType,
                Order = this.Order.Value,
                Sets = this.Sets.Select(s => s.ToDto()).ToList()
            };
    };
}