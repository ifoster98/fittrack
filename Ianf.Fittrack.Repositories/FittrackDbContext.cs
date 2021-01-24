using Ianf.Fittrack.Repositories.Entities;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Ianf.Fittrack.Repositories
{
    public partial class FittrackDbContext : DbContext
    {
        public FittrackDbContext()
        {
        }

        public FittrackDbContext(DbContextOptions<FittrackDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ActualWorkout> ActualWorkouts { get; set; }
        public virtual DbSet<Exercise> Exercises { get; set; }
        public virtual DbSet<PlannedWorkout> PlannedWorkouts { get; set; }
        public virtual DbSet<ExerciseEntry> ExerciseEntries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=192.168.1.73; Database=Fittrack; User Id=SA; Password=31Freeble$");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<ActualWorkout>(entity =>
            {
                entity.ToTable("ActualWorkout");

                entity.Property(e => e.ProgramName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.WorkoutTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<Exercise>(entity =>
            {
                entity.ToTable("Exercise");

                entity.HasOne(d => d.ActualWorkout)
                    .WithMany(p => p.Exercises)
                    .HasForeignKey(d => d.ActualWorkoutId)
                    .HasConstraintName("FK_Actual_Workout_Id");

                entity.HasOne(d => d.PlannedWorkout)
                    .WithMany(p => p.Exercises)
                    .HasForeignKey(d => d.PlannedWorkoutId)
                    .HasConstraintName("FK_Exercise_Planned_Workout_Id");
            });

            modelBuilder.Entity<PlannedWorkout>(entity =>
            {
                entity.ToTable("PlannedWorkout");

                entity.Property(e => e.ProgramName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.WorkoutTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<ExerciseEntry>(entity =>
            {
                entity.ToTable("ExerciseEntry");

                entity.Property(e => e.Weight).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Exercise)
                    .WithMany(p => p.ExerciseEntries)
                    .HasForeignKey(d => d.ExerciseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Exercise_Id");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
