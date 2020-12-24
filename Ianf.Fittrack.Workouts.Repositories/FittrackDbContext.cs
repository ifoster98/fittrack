using Ianf.Fittrack.Workouts.Repositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ianf.Fittrack.Workouts.Repositories
{
    public class FittrackDbContext : DbContext{
        public FittrackDbContext(DbContextOptions<FittrackDbContext> options) : base(options)
        {

        }

        public DbSet<Set> Sets { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Workout> Workouts { get; set; }
    }
}