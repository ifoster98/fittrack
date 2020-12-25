using Ianf.Fittrack.Workouts.Persistance.Interfaces;
using Ianf.Fittrack.Workouts.Domain;
using System;
using System.Linq;
using LanguageExt;
using System.Threading.Tasks;
using static Ianf.Fittrack.Workouts.Repositories.Convert;
using static LanguageExt.Prelude;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Ianf.Fittrack.Workouts.Repositories
{
    public class WorkoutRepository : IWorkoutRepository
    {
        protected FittrackDbContext _dbContext { get; }

        public WorkoutRepository(FittrackDbContext context) => _dbContext = context;

        public async Task<PositiveInt> SaveWorkoutAsync(Domain.Workout workout)
        {
            var entity = workout.ToEntity();
            _dbContext.Workouts.Add(entity);
            await _dbContext.SaveChangesAsync();
            return PositiveInt.CreatePositiveInt(entity.Id).IfNone(new PositiveInt());
        } 

        public async Task<List<Workout>> GetWorkoutsAfterDate(DateTime workoutDate) => 
            await _dbContext.Workouts
                .OrderBy(s => s.WorkoutTime)
                .Where(s => s.WorkoutTime > workoutDate)
                .Select(s => s.ToDomain())
                .ToListAsync();
    }
}