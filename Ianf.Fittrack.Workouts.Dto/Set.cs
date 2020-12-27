namespace Ianf.Fittrack.Workouts.Dto
{
    public struct Set
    {
        public int Reps { get; set; }
        public decimal Weight { get; set; }
        public int Order { get; set; }

        public override bool Equals(object? obj)
        {
            if(obj is null) return false;
            var item = (Set)obj;
            return item.Reps.Equals(Reps) && item.Weight.Equals(Weight) && item.Order.Equals(Order);
        }

        public override int GetHashCode()
        {
            throw new System.NotImplementedException();
        }
    }
}