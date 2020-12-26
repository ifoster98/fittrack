using System;
using System.Collections.Generic;
using Ianf.Fittrack.Workouts.Domain;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Linq;

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
        
        private Workout GetSampleWorkout() => 
            new Workout(
                1,
                ProgramName.CreateProgramName("Workout1").IfNone(new ProgramName()),
                DateTime.UtcNow.AddDays(-1),
                new List<Exercise>()
                {
                    new Exercise(
                        ExerciseType.Deadlift,
                        new List<Set>()
                        {
                            new Set(
                                PositiveInt.CreatePositiveInt(5).IfNone(new PositiveInt()),
                                Weight.CreateWeight(130.0M).IfNone(new Weight()),
                                PositiveInt.CreatePositiveInt(1).IfNone(new PositiveInt()) 
                            )
                        },
                        PositiveInt.CreatePositiveInt(1).IfNone(new PositiveInt())
                    )
                }
            );

        [Fact]
        public async void TestSaveWorkout() 
        {
            using(var context = new FittrackDbContext(ContextOptions))
            {
                // Assemble
                var workout = GetSampleWorkout() with { WorkoutTime = DateTime.Now.AddDays(-1) }; 
                var repository = new WorkoutRepository(context);

                // Act
                await repository.SaveWorkoutAsync(workout);

                // Assert
                var workoutCount = await context.Workouts.CountAsync();
                Assert.Equal(1, workoutCount);
                var result = await context.Workouts.FirstAsync();
                Assert.NotNull(result);
                Assert.Equal(1, result.Exercises.Count());
                Assert.Equal(1, result.Exercises.First().Sets.Count());
            }
        }

        [Fact]
        public async void TestGetWorkoutsAfterDate()
        {
            using(var context = new FittrackDbContext(ContextOptions))
            {
                // Assemble
                var repository = new WorkoutRepository(context);
                var workout = GetSampleWorkout();
                await repository.SaveWorkoutAsync(workout);
                await repository.SaveWorkoutAsync(GetSampleWorkout() with { WorkoutTime = DateTime.Now.AddDays(-1) });
                await repository.SaveWorkoutAsync(GetSampleWorkout() with { WorkoutTime = DateTime.Now.AddDays(1) });
                await repository.SaveWorkoutAsync(GetSampleWorkout() with { WorkoutTime = DateTime.Now.AddDays(2) });

                // Act
                var workouts = await repository.GetWorkoutsAfterDate(DateTime.UtcNow);

                // Assert
                Assert.Equal(2, workouts.Count());
            }
        }
    }
}