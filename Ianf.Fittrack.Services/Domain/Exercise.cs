using System.Collections.Generic;
using System.Linq;
using Ianf.Fittrack.Services.Dto;
using LanguageExt;

namespace Ianf.Fittrack.Services.Domain
{
    public record Exercise(ExerciseType ExerciseType, List<ExerciseEntry> ExerciseEntries, PositiveInt Order) { 

        public Dto.Exercise ToDto() =>
            new Dto.Exercise() {
                ExerciseType = this.ExerciseType,
                Order = this.Order.Value,
                ExerciseEntries = this.ExerciseEntries.Select(s => s.ToDto()).ToList()
            };
    };
}