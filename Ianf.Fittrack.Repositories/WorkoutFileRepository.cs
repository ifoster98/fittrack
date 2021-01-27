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
        private string dataDirectory = "data";
        private string dataFile = "fittrack.json";

        public List<Workout> GetWorkoutsOnOrAfterDate(DateTime workoutDate)
        {
            var context = GetFittrackFileContext();
            return context.Workouts.Where(w => w.WorkoutTime >= workoutDate).ToList();
        }

        public bool HasWorkout(DateTime workoutDate, Services.Dto.ProgramType programType, ProgramName programName)
        {
            var context = GetFittrackFileContext();
            return context.Workouts.Any(p => p.WorkoutTime.Equals(workoutDate) && p.ProgramType.Equals(programType) && p.ProgramName.Equals(programName));
        }

        public PositiveInt AddWorkout(Workout workout)
        {
            var context = GetFittrackFileContext();
            context.Workouts.Add(workout);
            SaveFittrackFileContext(context);
            return PositiveInt.CreatePositiveInt(1).IfNone(new PositiveInt());
        }

        private FittrackFileContext GetFittrackFileContext() 
        {
            var storage = $"{dataDirectory}/{dataFile}";
            if(!File.Exists(storage)) return new FittrackFileContext();
            return JsonConvert.DeserializeObject<FittrackFileContext>(File.ReadAllText(storage));
        }

        private void SaveFittrackFileContext(FittrackFileContext context) {
            if(!Directory.Exists(dataDirectory)) Directory.CreateDirectory(dataDirectory);
            var storage = $"{dataDirectory}/{dataFile}";
            File.WriteAllText(storage, JsonConvert.SerializeObject(context));
        }
    }
}