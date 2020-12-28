using Ianf.Fittrack.Workouts.Generators.Interfaces;
using Ianf.Fittrack.Workouts.Dto;

namespace Ianf.Fittrack.Workouts.Generators
{
    public class UpperLowerGenerator : IGenerator
    {
        public UpperLowerGenerator()
        {
        }

        public Domain.PlannedWorkout GetNextWorkout(Domain.PlannedWorkout plannedWorkout, Domain.ActualWorkout actualworkout)
        {
            return plannedWorkout;
        }

        public ProgramType GetProgramType() => ProgramType.UpperLower;
    }
}