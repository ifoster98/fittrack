using Ianf.Fittrack.Workouts.Persistance.Interfaces;
using Ianf.Fittrack.Workouts.Domain;
using System;
using System.Linq;
using LanguageExt;
using System.Threading.Tasks;
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
                .Include(w => w.Exercises)
                    .ThenInclude(e => e.Sets)
                .Where(s => s.WorkoutTime > workoutDate)
                .OrderBy(s => s.WorkoutTime)
                .Select(s => s.ToDomain())
                .ToListAsync();

        public async Task<bool> HasWorkout(DateTime workoutDate, ProgramName programName) =>
            await _dbContext.Workouts
                .Where(w => w.WorkoutTime.Date == workoutDate.Date 
                    && w.ProgramName == programName.Value)
                .AnyAsync();
    }
}