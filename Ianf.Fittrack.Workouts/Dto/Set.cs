using Ianf.Fittrack.Workouts.Domain;

namespace Ianf.Fittrack.Dto
{
    public struct Set
    {
        public ExerciseType ExerciseType { get; set; }
        public int Reps { get; set; }
        public double Weight { get; set; }
        public int Order { get; set; }
    }
}