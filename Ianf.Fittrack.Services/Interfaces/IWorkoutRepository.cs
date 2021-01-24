using System;
using System.Collections.Generic;
using Ianf.Fittrack.Services.Domain;

namespace Ianf.Fittrack.Services.Interfaces
{
    public interface IWorkoutRepository
    {
        PositiveInt AddWorkout(PlannedWorkout workout);
        PositiveInt AddWorkout(ActualWorkout workout);
        List<PlannedWorkout> GetPlannedWorkoutsAfterDate(DateTime workoutDate);
        bool HasWorkout(DateTime workoutDate, Services.Dto.ProgramType programType, ProgramName programName);
    }
}
