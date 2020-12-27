#nullable disable
using System;
using System.Collections.Generic;
using Ianf.Fittrack.Workouts.Domain;
using Ianf.Fittrack.Workouts.Persistance.Interfaces;
using Ianf.Fittrack.Workouts.Services;
using Ianf.Fittrack.Workouts.Services.Interfaces;
using Moq;
using Xunit;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Ianf.Fittrack.Workouts.Dto;

namespace Ianf.Fittrack.UnitTest.Services
{
    public class WorkoutServiceTests
    {
        private readonly Mock<IWorkoutRepository> _workoutRepository;
        private readonly Mock<ILogger> _logger;
        private readonly IWorkoutService _workoutService;
        private DateTime workoutTime = DateTime.Now;

        public Fittrack.Workouts.Dto.Workout GetSampleWorkout() =>
            new Fittrack.Workouts.Dto.Workout() 
            {
                ProgramName = "Workout1",
                WorkoutTime = workoutTime,
                Exercises = new List<Fittrack.Workouts.Dto.Exercise>()
                {
                    new Fittrack.Workouts.Dto.Exercise()
                    {
                        ExerciseType = ExerciseType.Deadlift,
                        Order = 1,
                        Sets = new List<Fittrack.Workouts.Dto.Set>()
                        {
                            new Fittrack.Workouts.Dto.Set()
                            {
                                Reps = 5,
                                Weight = 130,
                                Order = 1
                            }
                        }
                    }
                }
            };

        public WorkoutServiceTests()
        {
            _workoutRepository = new Mock<IWorkoutRepository>();
            _logger = new Mock<ILogger>();
            _workoutService = new WorkoutService(_workoutRepository.Object);
        }

        [Fact]
        public async void TestAddNewWorkoutAsyncSuccess()
        {
            // Assemble
            var newWorkout = GetSampleWorkout();
            _workoutRepository
                .Setup(w => w.SaveWorkoutAsync(It.IsAny<Fittrack.Workouts.Domain.Workout>()))
                .Returns(Task.FromResult(PositiveInt.CreatePositiveInt(1).IfNone(new PositiveInt())));

            // Act
            var result = await _workoutService.AddNewWorkoutAsync(newWorkout);

            // Assert
            _workoutRepository.Verify(w => w.SaveWorkoutAsync(It.IsAny<Fittrack.Workouts.Domain.Workout>()));
            result.Match(
                Left: (err) => Assert.False(true, "Expected no errors to be returned."),
                Right: (newId) => Assert.Equal(1, newId.Value)
            );
        }

        [Fact]
        public async void TestAddNewWorkoutAsyncFailsIfProgramNameIsEmpty()
        {
            // Assemble
            var newWorkout = GetSampleWorkout();
            newWorkout.ProgramName = string.Empty;

            // Act
            var result = await _workoutService.AddNewWorkoutAsync(newWorkout);

            // Assert
            result.Match(
                Left: (err) => {
                    Assert.Equal("Workout", err.First().DtoType);
                    Assert.Equal("ProgramName", err.First().DtoProperty);
                },
                Right: (newId) => Assert.False(true, "Expected error.")
            );
        }

        [Fact]
        public async void TestAddNewWorkoutAsyncFailsIfExerciseOrderIsNotPositive()
        {
            // Assemble
            var newWorkout = GetSampleWorkout();
            var exercise = newWorkout.Exercises.First();
            exercise.Order = 0;
            newWorkout.Exercises.Clear();
            newWorkout.Exercises.Add(exercise);

            // Act
            var result = await _workoutService.AddNewWorkoutAsync(newWorkout);

            // Assert
            result.Match(
                Left: (err) => {
                    Assert.Equal("Exercise", err.First().DtoType);
                    Assert.Equal("Order", err.First().DtoProperty);
                },
                Right: (newId) => Assert.False(true, "Expected error.")
            );
        }

        [Fact]
        public async void TestAddNewWorkoutAsyncFailsIfSetRepsIsNotPositive()
        {
            // Assemble
            var newWorkout = GetSampleWorkout();
            var exercise = newWorkout.Exercises.First();
            var set = exercise.Sets.First();
            set.Reps = -2;
            exercise.Sets.Clear();
            exercise.Sets.Add(set);

            // Act
            var result = await _workoutService.AddNewWorkoutAsync(newWorkout);

            // Assert
            result.Match(
                Left: (err) => {
                    Assert.Equal("Set", err.First().DtoType);
                    Assert.Equal("Reps", err.First().DtoProperty);
                },
                Right: (newId) => Assert.False(true, "Expected error.")
            );
        }

        [Fact]
        public async void TestAddNewWorkoutAsyncFailsIfSetWeightIsNotValid()
        {
            // Assemble
            var newWorkout = GetSampleWorkout();
            var exercise = newWorkout.Exercises.First();
            var set = exercise.Sets.First();
            set.Weight = 130.1234M;
            exercise.Sets.Clear();
            exercise.Sets.Add(set);

            // Act
            var result = await _workoutService.AddNewWorkoutAsync(newWorkout);

            // Assert
            result.Match(
                Left: (err) => {
                    Assert.Equal("Set", err.First().DtoType);
                    Assert.Equal("Weight", err.First().DtoProperty);
                },
                Right: (newId) => Assert.False(true, "Expected error.")
            );
        }

        [Fact]
        public async void TestAddNewWorkoutAsyncFailsIfSetOrderIsNotPositive()
        {
            // Assemble
            var newWorkout = GetSampleWorkout();
            var exercise = newWorkout.Exercises.First();
            var set = exercise.Sets.First();
            set.Order = -4;
            exercise.Sets.Clear();
            exercise.Sets.Add(set);

            // Act
            var result = await _workoutService.AddNewWorkoutAsync(newWorkout);

            // Assert
            result.Match(
                Left: (err) => {
                    Assert.Equal("Set", err.First().DtoType);
                    Assert.Equal("Order", err.First().DtoProperty);
                },
                Right: (newId) => Assert.False(true, "Expected error.")
            );
        }

        [Fact]
        public async void TestAddNewWorkoutAsyncFailsIfExercisesListIsNull()
        {
            // Assemble
            var newWorkout = new Fittrack.Workouts.Dto.Workout()
            {
                ProgramName = "Workout1",
                WorkoutTime = workoutTime,
                Exercises = null
            };

            // Act
            var result = await _workoutService.AddNewWorkoutAsync(newWorkout);

            // Assert
            result.Match(
                Left: (err) => {
                    Assert.Equal("Workout", err.First().DtoType);
                    Assert.Equal("Exercises", err.First().DtoProperty);
                },
                Right: (newId) => Assert.False(true, "Expected error.")
            );
        }

        [Fact]
        public async void TestAddNewWorkoutAsyncFailsIfExercisesListHasNoExercises()
        {
            // Assemble
            var newWorkout = new Fittrack.Workouts.Dto.Workout()
            {
                ProgramName = "Workout1",
                WorkoutTime = workoutTime,
                Exercises = new List<Fittrack.Workouts.Dto.Exercise>()
            };

            // Act
            var result = await _workoutService.AddNewWorkoutAsync(newWorkout);

            // Assert
            result.Match(
                Left: (err) => {
                    Assert.Equal("Workout", err.First().DtoType);
                    Assert.Equal("Exercises", err.First().DtoProperty);
                },
                Right: (newId) => Assert.False(true, "Expected error.")
            );
        }

        [Fact]
        public async void TestGetNextWorkout()
        {
            // Assemble
            var timestamp = DateTime.Now;
            var newWorkout = GetSampleWorkout();
            newWorkout.WorkoutTime = timestamp.AddDays(1);
            var workout = newWorkout.Validate().IfLeft(new Fittrack.Workouts.Domain.Workout(
                1,
                ProgramName.CreateProgramName("test").IfNone(new ProgramName()),
                DateTime.Now,
                new List<Fittrack.Workouts.Domain.Exercise>()
                ));
            var workoutTwo = workout with { WorkoutTime = timestamp.AddDays(2) };

            _workoutRepository.Setup(w => w.GetWorkoutsAfterDate(It.IsAny<DateTime>())).Returns(Task.FromResult(new List<Fittrack.Workouts.Domain.Workout>
            {
                workout, workoutTwo
            }));

            // Act
            var nextWorkout = await _workoutService.GetNextWorkoutAsync(DateTime.Now);

            // Assert
            nextWorkout.Match
            (
                None: () => Assert.False(true, "Expected result"),
                Some: (s) => {
                    Assert.Equal(newWorkout.ProgramName, s.ProgramName);
                    Assert.Equal(newWorkout.WorkoutTime, s.WorkoutTime);
                    Assert.Equal(newWorkout.Exercises.Count(), s.Exercises.Count());
                }
            );
        }

        [Fact]
        public async void TestGetNextWorkoutReturnsNoneIfNoWorkouts()
        {
            // Assemble
            _workoutRepository.Setup(w => w.GetWorkoutsAfterDate(It.IsAny<DateTime>())).Returns(Task.FromResult(new List<Fittrack.Workouts.Domain.Workout>
            {
            }));

            // Act
            var nextWorkout = await _workoutService.GetNextWorkoutAsync(DateTime.Now);

            // Assert
            nextWorkout.Match
            (
                None: () => Assert.True(true, ""),
                Some: (s) => Assert.False(true, $"Expected 'None' return. Got {s}.")
            );
        }

        [Fact]
        public async void TestGetNextWorkoutReturnsNoneIfDatePassedInIsMaxValue()
        {
            // Assemble
            var timestamp = DateTime.Now;
            var newWorkout = GetSampleWorkout();
            newWorkout.WorkoutTime = timestamp.AddDays(1);
            var workout = newWorkout.Validate().IfLeft(new Fittrack.Workouts.Domain.Workout(
                1,
                ProgramName.CreateProgramName("test").IfNone(new ProgramName()),
                DateTime.Now,
                new List<Fittrack.Workouts.Domain.Exercise>()
                ));
            var workoutTwo = workout with { WorkoutTime = timestamp.AddDays(2) };

            _workoutRepository.Setup(w => w.GetWorkoutsAfterDate(It.IsAny<DateTime>())).Returns(Task.FromResult(new List<Fittrack.Workouts.Domain.Workout>
            {
                workout, workoutTwo
            }));

            // Act
            var nextWorkout = await _workoutService.GetNextWorkoutAsync(DateTime.MaxValue);

            // Assert
            nextWorkout.Match
            (
                None: () => Assert.True(true, ""),
                Some: (s) => Assert.False(true, $"Expected 'None' return. Got {s}.")
            );
        }

        [Fact]
        public async void TestGetNextWorkoutReturnsNoneIfDatePassedInIsMinValue()
        {
            // Assemble
            var timestamp = DateTime.Now;
            var newWorkout = GetSampleWorkout();
            newWorkout.WorkoutTime = timestamp.AddDays(1);
            var workout = newWorkout.Validate().IfLeft(new Fittrack.Workouts.Domain.Workout(
                1,
                ProgramName.CreateProgramName("test").IfNone(new ProgramName()),
                DateTime.Now,
                new List<Fittrack.Workouts.Domain.Exercise>()
                ));
            var workoutTwo = workout with { WorkoutTime = timestamp.AddDays(2) };

            _workoutRepository.Setup(w => w.GetWorkoutsAfterDate(It.IsAny<DateTime>())).Returns(Task.FromResult(new List<Fittrack.Workouts.Domain.Workout>
            {
                workout, workoutTwo
            }));

            // Act
            var nextWorkout = await _workoutService.GetNextWorkoutAsync(DateTime.MinValue);

            // Assert
            nextWorkout.Match
            (
                None: () => Assert.True(true, ""),
                Some: (s) => Assert.False(true, $"Expected 'None' return. Got {s}.")
            );
        }
    }
}