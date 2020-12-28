using System;
using System.Collections.Generic;
using Ianf.Fittrack.Workouts.Domain;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Linq;
using Ianf.Fittrack.Workouts.Dto;

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
        
        private Domain.PlannedWorkout GetSamplePlannedWorkout() => 
            new Domain.PlannedWorkout(
                1,
                ProgramName.CreateProgramName("Workout1").IfNone(new ProgramName()),
                DateTime.UtcNow.AddDays(-1),
                new List<Domain.Exercise>()
                {
                    new Domain.Exercise(
                        ExerciseType.Deadlift,
                        new List<Domain.Set>()
                        {
                            new Domain.Set(
                                PositiveInt.CreatePositiveInt(5).IfNone(new PositiveInt()),
                                Weight.CreateWeight(130.0M).IfNone(new Weight()),
                                PositiveInt.CreatePositiveInt(1).IfNone(new PositiveInt()) 
                            )
                        },
                        PositiveInt.CreatePositiveInt(1).IfNone(new PositiveInt())
                    )
                }
            );

        private Domain.ActualWorkout GetSampleActualWorkout() { 
            var plannedWorkout = GetSamplePlannedWorkout();
            var actualWorkout = new Domain.ActualWorkout(
                1,
                plannedWorkout,
                ProgramName.CreateProgramName("Workout1").IfNone(new ProgramName()),
                DateTime.UtcNow.AddDays(-1),
                new List<Domain.Exercise>()
                {
                    new Domain.Exercise(
                        ExerciseType.Deadlift,
                        new List<Domain.Set>()
                        {
                            new Domain.Set(
                                PositiveInt.CreatePositiveInt(5).IfNone(new PositiveInt()),
                                Weight.CreateWeight(130.0M).IfNone(new Weight()),
                                PositiveInt.CreatePositiveInt(1).IfNone(new PositiveInt()) 
                            )
                        },
                        PositiveInt.CreatePositiveInt(1).IfNone(new PositiveInt())
                    )
                }
            );
            return actualWorkout;
        }

        [Fact]
        public async void TestSavePlannedWorkout() 
        {
            using(var context = new FittrackDbContext(ContextOptions))
            {
                // Assemble
                var workout = GetSamplePlannedWorkout() with { WorkoutTime = DateTime.Now.AddDays(-1) }; 
                var repository = new WorkoutRepository(context);

                // Act
                await repository.SaveWorkoutAsync(workout);

                // Assert
                var workoutCount = await context.PlannedWorkouts.CountAsync();
                Assert.Equal(1, workoutCount);
                var result = await context.PlannedWorkouts.FirstAsync();
                Assert.NotNull(result);
                Assert.Single(result.Exercises);
                Assert.Single(result.Exercises.First().Sets);
            }
        }

        [Fact]
        public async void TestSaveActualWorkout() 
        {
            using(var context = new FittrackDbContext(ContextOptions))
            {
                // Assemble
                var workout = GetSampleActualWorkout() with { WorkoutTime = DateTime.Now.AddDays(-1) }; 
                var repository = new WorkoutRepository(context);

                // Act
                await repository.SaveWorkoutAsync(workout);

                // Assert
                var workoutCount = await context.ActualWorkouts.CountAsync();
                Assert.Equal(1, workoutCount);
                var result = await context.ActualWorkouts.FirstAsync();
                Assert.NotNull(result);
                Assert.Single(result.Exercises);
                Assert.Single(result.Exercises.First().Sets);

            }
        }

        [Fact]
        public async void TestGetWorkoutsAfterDate()
        {
            using(var context = new FittrackDbContext(ContextOptions))
            {
                // Assemble
                var repository = new WorkoutRepository(context);
                var workout = GetSamplePlannedWorkout();
                await repository.SaveWorkoutAsync(workout);
                await repository.SaveWorkoutAsync(GetSamplePlannedWorkout() with { WorkoutTime = DateTime.Now.AddDays(-1) });
                await repository.SaveWorkoutAsync(GetSamplePlannedWorkout() with { WorkoutTime = DateTime.Now.AddDays(1) });
                await repository.SaveWorkoutAsync(GetSamplePlannedWorkout() with { WorkoutTime = DateTime.Now.AddDays(2) });

                // Act
                var workouts = await repository.GetWorkoutsAfterDate(DateTime.UtcNow);

                // Assert
                Assert.Equal(2, workouts.Count());
            }
        }
    }
}