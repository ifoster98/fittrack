using Ianf.Fittrack.Workouts.Domain;
using Ianf.Fittrack.Workouts.Dto;

namespace Ianf.Fittrack.Workouts.Generators.Interfaces
{
    public interface IGenerator
    {
        Domain.PlannedWorkout GetNextWorkout(Domain.PlannedWorkout plannedWorkout, Domain.ActualWorkout actualworkout);

        ProgramType GetProgramType();
    }
}