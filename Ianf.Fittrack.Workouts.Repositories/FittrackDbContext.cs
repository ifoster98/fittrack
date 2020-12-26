using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Ianf.Fittrack.Workouts.Repositories.Entities;

#nullable disable

namespace Ianf.Fittrack.Workouts.Repositories
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

        public virtual DbSet<Exercise> Exercises { get; set; }
        public virtual DbSet<Set> Sets { get; set; }
        public virtual DbSet<Workout> Workouts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Exercise>(entity =>
            {
                entity.ToTable("Exercise");

                entity.HasOne(d => d.Workout)
                    .WithMany(p => p.Exercises)
                    .HasForeignKey(d => d.WorkoutId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Workout_Id");
            });

            modelBuilder.Entity<Set>(entity =>
            {
                entity.ToTable("Set");

                entity.Property(e => e.Weight).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Exercise)
                    .WithMany(p => p.Sets)
                    .HasForeignKey(d => d.ExerciseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Exercise_Id");
            });

            modelBuilder.Entity<Workout>(entity =>
            {
                entity.ToTable("Workout");

                entity.Property(e => e.ProgramName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.WorkoutTime).HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
