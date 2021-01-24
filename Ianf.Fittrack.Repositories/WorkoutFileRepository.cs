using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using Ianf.Fittrack.Services.Domain;
using Ianf.Fittrack.Services.Interfaces;
using Newtonsoft.Json;

namespace Ianf.Fittrack.Repositories
{
    public class WorkoutFileRepository : IWorkoutRepository
    {
        private string dataFile = "fittrack.json";

        public List<PlannedWorkout> GetPlannedWorkoutsAfterDate(DateTime workoutDate)
        {
            throw new NotImplementedException();
        }

        public bool HasWorkout(DateTime workoutDate, Services.Dto.ProgramType programType, ProgramName programName)
        {
            var context = GetFittrackFileContext();
            return context.PlannedWorkouts.Any(p => p.WorkoutTime.Equals(workoutDate) && p.ProgramType.Equals(programType) && p.ProgramName.Equals(programName));
        }

        public PositiveInt AddWorkout(PlannedWorkout workout)
        {
            var context = GetFittrackFileContext();
            context.PlannedWorkouts.Add(workout);
            SaveFittrackFileContext(context);
            return PositiveInt.CreatePositiveInt(1).IfNone(new PositiveInt());
        }

        public PositiveInt AddWorkout(ActualWorkout workout)
        {
            var context = GetFittrackFileContext();
            context.ActualWorkouts.Add(workout);
            SaveFittrackFileContext(context);
            return PositiveInt.CreatePositiveInt(1).IfNone(new PositiveInt());
        }

        private FittrackFileContext GetFittrackFileContext() 
        {
            if(!File.Exists(dataFile)) return new FittrackFileContext();
            return JsonConvert.DeserializeObject<FittrackFileContext>(File.ReadAllText(dataFile));
        }

        private void SaveFittrackFileContext(FittrackFileContext context) =>
            File.WriteAllText(dataFile, JsonConvert.SerializeObject(context));
    }
}