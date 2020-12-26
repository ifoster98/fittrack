namespace Ianf.Fittrack.Workouts.Domain
{
    public record Set(PositiveInt Reps, Weight Weight, PositiveInt Order) 
    { 
        public Dto.Set ToDto()  =>
            new Dto.Set() {
                Reps = this.Reps.Value,
                Weight = this.Weight.Value,
                Order = this.Order.Value
            };
    }
}