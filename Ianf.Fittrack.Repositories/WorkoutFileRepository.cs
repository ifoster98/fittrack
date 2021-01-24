using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ianf.Fittrack.Services.Domain;
using Ianf.Fittrack.Services.Interfaces;

namespace Ianf.Fittrack.Repositories
 {
    public class WorkoutFileRepository : IWorkoutRepository
    {
        public Task<List<PlannedWorkout>> GetWorkoutsAfterDate(DateTime workoutDate)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasWorkout(DateTime workoutDate, ProgramName programName)
        {
            throw new NotImplementedException();
        }

        public Task<PositiveInt> SaveWorkoutAsync(PlannedWorkout workout)
        {
            throw new NotImplementedException();
        }

        public Task<PositiveInt> SaveWorkoutAsync(ActualWorkout workout)
        {
            throw new NotImplementedException();
        }
    }
}