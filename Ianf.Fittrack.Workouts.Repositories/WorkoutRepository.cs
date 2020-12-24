using Ianf.Fittrack.Workouts.Persistance.Interfaces;
using Ianf.Fittrack.Workouts.Domain;
using System;
using System.Collections.Generic;
using LanguageExt;
using System.Threading.Tasks;

namespace Ianf.Fittrack.Workouts.Repositories
{
    public class WorkoutRepository : IWorkoutRepository
    {
        protected FittrackDbContext _dbContext { get; }

        public WorkoutRepository(FittrackDbContext context) => _dbContext = context;

        public async Task<PositiveInt> SaveWorkoutAsync(Workout workout)
        {
            var entity = workout.ToEntity();
            _dbContext.Workout.Add(entity);
            _dbContext.SaveChanges();
            return PositiveInt.CreatePositiveInt(entity.Id).IfNone(new PositiveInt());
        } 

        public Task<Option<Workout>> GetNextWorkoutAsync()
        {
            throw new NotImplementedException();
        }
    }
}