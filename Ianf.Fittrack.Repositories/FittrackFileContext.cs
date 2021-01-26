using System.Collections.Generic;
using Ianf.Fittrack.Services.Domain;

namespace Ianf.Fittrack.Repositories
{
    public class FittrackFileContext
    {
        public List<Workout> Workouts { get; set; }

        public FittrackFileContext()
        {
            Workouts = new List<Workout>();
        }
    }
}
