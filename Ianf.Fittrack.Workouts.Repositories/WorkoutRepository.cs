using Ianf.Fittrack.Workouts.Persistance.Interfaces;
using Ianf.Fittrack.Workouts.Domain;
using System;
using System.Collections.Generic;
using LanguageExt;

namespace Ianf.Fittrack.Workouts.Repositories
{
    public class WorkoutRepository : IWorkoutRepository
    {
        protected FittrackDbContext _dbContext { get; }

        public WorkoutRepository(FittrackDbContext context) => _dbContext = context;

        public PositiveInt SaveWorkout(Workout workout)
        {
            var entity = workout.ToEntity();
            _dbContext.Workout.Add(entity);
            _dbContext.SaveChanges();
            return PositiveInt.CreatePositiveInt(entity.Id).IfNone(new PositiveInt());
        } 

        public Option<Workout> GetNextWorkout()
        {
            var programName = ProgramName.CreateProgramName("Test Program").IfNone(new ProgramName());
            var workoutTime = DateTime.Now;
            var plannedExercises = new List<Exercise>();
            return new Workout(programName, workoutTime, plannedExercises);
        }
    }
}