namespace Ianf.Fittrack.Services.Domain
{
    public record Set(PositiveInt PlannedReps, Weight PlannedWeight, NonNegativeInt ActualReps, Weight ActualWeight, PositiveInt Order) 
    { 
        public Dto.Set ToDto()  =>
            new Dto.Set() {
                PlannedReps = this.PlannedReps.Value,
                PlannedWeight = this.PlannedWeight.Value,
                ActualReps = this.ActualReps.Value,
                ActualWeight = this.ActualWeight.Value,
                Order = this.Order.Value
            };
    }
}