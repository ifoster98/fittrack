namespace Ianf.Fittrack.Services.Dto
{
    public struct Set
    {
        public int PlannedReps { get; set; }
        public decimal PlannedWeight { get; set; }
        public int ActualReps { get; set; }
        public decimal ActualWeight { get; set; }
        public int Order { get; set; }
    }
}