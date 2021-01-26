using System;
using System.Collections.Generic;
using Ianf.Fittrack.Services.Domain;

namespace Ianf.Fittrack.Services.Interfaces
{
    public interface IWorkoutRepository
    {
        PositiveInt AddWorkout(Workout workout);
        List<Workout> GetWorkoutsAfterDate(DateTime workoutDate);
        bool HasWorkout(DateTime workoutDate, Services.Dto.ProgramType programType, ProgramName programName);
    }
}
