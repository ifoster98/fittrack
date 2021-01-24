using System.Collections.Generic;
using Ianf.Fittrack.Services.Domain;

namespace Ianf.Fittrack.Repositories
{
    public class FittrackFileContext
    {
        public List<PlannedWorkout> PlannedWorkouts { get; set; }
        public List<ActualWorkout> ActualWorkouts { get; set; }

        public FittrackFileContext()
        {
            PlannedWorkouts = new List<PlannedWorkout>();
            ActualWorkouts = new List<ActualWorkout>();
        }
    }
}
