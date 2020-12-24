using System;
using System.Collections.Generic;
using Ianf.Fittrack.Workouts.Repositories;
using Ianf.Fittrack.Workouts.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Xunit;

namespace Ianf.Fittrack.Workouts.Repositories.Tests
{
    public class WorkoutRepositoryTests
    {
        private string connectionString = @"Server=192.168.1.73; Database=Fittrack; User Id=SA; Password=31Freeble$";

        public WorkoutRepositoryTests() => ContextOptions = new DbContextOptionsBuilder<FittrackDbContext>()
                .UseSqlServer(connectionString)
                .EnableSensitiveDataLogging()
                .LogTo(Console.WriteLine)
                .Options;

        protected DbContextOptions<FittrackDbContext> ContextOptions { get; }

        [Fact]
        public void TestSaveWorkout() 
        {
            using(var context = new FittrackDbContext(ContextOptions))
            {
                // Assemble
                var workout = new Workout(
                    ProgramName.CreateProgramName("Workout1").IfNone(new ProgramName()),
                    DateTime.Now,
                    new List<Exercise>()
                    {
                        new Exercise(
                            ExerciseType.Deadlift,
                            new List<Set>()
                            {
                                new Set(
                                    PositiveInt.CreatePositiveInt(5).IfNone(new PositiveInt()),
                                    Weight.CreateWeight(130.0).IfNone(new Weight()),
                                    PositiveInt.CreatePositiveInt(1).IfNone(new PositiveInt()) 
                                )
                            },
                            PositiveInt.CreatePositiveInt(1).IfNone(new PositiveInt())
                        )
                    }
                );

                // Act
                var repository = new WorkoutRepository(context);
                repository.SaveWorkout(workout);

                // Assert
                Assert.Single(context.Workouts);
            }
        }
    }
}