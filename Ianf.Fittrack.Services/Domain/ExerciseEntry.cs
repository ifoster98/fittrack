namespace Ianf.Fittrack.Services.Domain
{
    public record ExerciseEntry(PositiveInt Reps, Weight Weight, PositiveInt Order) 
    { 
        public Dto.ExerciseEntry ToDto()  =>
            new Dto.ExerciseEntry() {
                Reps = this.Reps.Value,
                Weight = this.Weight.Value,
                Order = this.Order.Value
            };
    }
}