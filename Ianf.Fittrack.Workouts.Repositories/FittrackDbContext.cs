using Ianf.Fittrack.Workouts.Repositories.Entity;
using Microsoft.EntityFrameworkCore;

namespace Ianf.Fittrack.Workouts.Repositories
{
    public class FittrackDbContext : DbContext{
        public FittrackDbContext(DbContextOptions<FittrackDbContext> options) : base(options)
        {

        }

        public DbSet<Set> Set { get; set; }
        public DbSet<Exercise> Exercise { get; set; }
        public DbSet<Workout> Workout { get; set; }
    }
}