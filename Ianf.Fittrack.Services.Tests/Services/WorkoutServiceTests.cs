#nullable disable
using System;
using System.Collections.Generic;
using Moq;
using Xunit;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Ianf.Fittrack.Services;
using Ianf.Fittrack.Services.Domain;
using Ianf.Fittrack.Services.Dto;
using Ianf.Fittrack.Services.Interfaces;

namespace Ianf.Fittrack.Services.Tests.Services
{
    public class WorkoutServiceTests
    {
        private readonly Mock<IWorkoutRepository> _workoutRepository;
        private readonly Mock<ILogger> _logger;
        private readonly IWorkoutService _workoutService;
        private DateTime workoutTime = DateTime.Now;
        private readonly string programName = "Workout1";

        public Ianf.Fittrack.Services.Dto.Workout GetSampleWorkout() =>
            new Ianf.Fittrack.Services.Dto.Workout() 
            {
                ProgramName = programName,
                WorkoutTime = workoutTime,
                Exercises = new List<Ianf.Fittrack.Services.Dto.Exercise>()
                {
                    new Ianf.Fittrack.Services.Dto.Exercise()
                    {
                        ExerciseType = Ianf.Fittrack.Services.Dto.ExerciseType.Deadlift,
                        Order = 1,
                        Sets = new List<Ianf.Fittrack.Services.Dto.Set>()
                        {
                            new Ianf.Fittrack.Services.Dto.Set()
                            {
                                PlannedReps = 5,
                                PlannedWeight = 130,
                                ActualReps = 0,
                                ActualWeight = 0,
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
        public void TestAddWorkoutSuccess()
        {
            // Assemble
            var newWorkout = GetSampleWorkout();
            _workoutRepository
                .Setup(w => w.AddWorkout(It.IsAny<Ianf.Fittrack.Services.Domain.Workout>()))
                .Returns(Ianf.Fittrack.Services.Domain.PositiveInt.CreatePositiveInt(1).IfNone(new Ianf.Fittrack.Services.Domain.PositiveInt()));

            // Act
            var result = _workoutService.AddWorkout(newWorkout);

            // Assert
            _workoutRepository.Verify(w => w.AddWorkout(It.IsAny<Ianf.Fittrack.Services.Domain.Workout>()));
            result.Match(
                Left: (err) => Assert.False(true, "Expected no errors to be returned."),
                Right: (newId) => Assert.Equal(1, newId.Value)
            );
        }

        [Fact]
        public void TestAddWorkoutFailsIfProgramNameIsEmpty()
        {
            // Assemble
            var newWorkout = GetSampleWorkout();
            newWorkout.ProgramName = string.Empty;

            // Act
            var result = _workoutService.AddWorkout(newWorkout);

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
        public void TestAddWorkoutFailsIfProgramNameAndDateIsDuplicate()
        {
            // Assemble
            var newWorkout = GetSampleWorkout();
            _workoutRepository.Setup(w => w.HasWorkout(It.IsAny<DateTime>(), It.IsAny<Ianf.Fittrack.Services.Dto.ProgramType>(), It.IsAny<Ianf.Fittrack.Services.Domain.ProgramName>())).Returns(true);

            // Act
            var result = _workoutService.AddWorkout(newWorkout);

            // Assert
            result.Match(
                Left: (err) => {
                    Assert.Equal("Duplicate workout definition.", err.First().ErrorMessage);
                    Assert.Equal("Workout", err.First().DtoType);
                },
                Right: (newId) => Assert.False(true, "Expected error.")
            );
        }

        [Fact]
        public void TestAddWorkoutFailsIfExerciseOrderIsNotPositive()
        {
            // Assemble
            var newWorkout = GetSampleWorkout();
            var exercise = newWorkout.Exercises.First();
            exercise.Order = 0;
            newWorkout.Exercises.Clear();
            newWorkout.Exercises.Add(exercise);

            // Act
            var result = _workoutService.AddWorkout(newWorkout);

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
        public void TestAddWorkoutFailsIfSetRepsIsNotPositive()
        {
            // Assemble
            var newWorkout = GetSampleWorkout();
            var exercise = newWorkout.Exercises.First();
            var set = exercise.Sets.First();
            set.PlannedReps = -2;
            exercise.Sets.Clear();
            exercise.Sets.Add(set);

            // Act
            var result = _workoutService.AddWorkout(newWorkout);

            // Assert
            result.Match(
                Left: (err) => {
                    Assert.Equal("Set", err.First().DtoType);
                    Assert.Equal("PlannedReps", err.First().DtoProperty);
                },
                Right: (newId) => Assert.False(true, "Expected error.")
            );
        }

        [Fact]
        public void TestAddWorkoutFailsIfSetWeightIsNotValid()
        {
            // Assemble
            var newWorkout = GetSampleWorkout();
            var exercise = newWorkout.Exercises.First();
            var set = exercise.Sets.First();
            set.PlannedWeight = 130.1234M;
            exercise.Sets.Clear();
            exercise.Sets.Add(set);

            // Act
            var result = _workoutService.AddWorkout(newWorkout);

            // Assert
            result.Match(
                Left: (err) => {
                    Assert.Equal("Set", err.First().DtoType);
                    Assert.Equal("PlannedWeight", err.First().DtoProperty);
                },
                Right: (newId) => Assert.False(true, "Expected error.")
            );
        }

        [Fact]
        public void TestAddWorkoutFailsIfSetOrderIsNotPositive()
        {
            // Assemble
            var newWorkout = GetSampleWorkout();
            var exercise = newWorkout.Exercises.First();
            var set = exercise.Sets.First();
            set.Order = -4;
            exercise.Sets.Clear();
            exercise.Sets.Add(set);

            // Act
            var result = _workoutService.AddWorkout(newWorkout);

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
        public void TestAddWorkoutFailsIfExercisesListIsNull()
        {
            // Assemble
            var newWorkout = new Ianf.Fittrack.Services.Dto.Workout()
            {
                ProgramName = programName,
                WorkoutTime = workoutTime,
                Exercises = null
            };

            // Act
            var result = _workoutService.AddWorkout(newWorkout);

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
        public void TestAddWorkoutFailsIfExercisesListHasNoExercises()
        {
            // Assemble
            var newWorkout = new Ianf.Fittrack.Services.Dto.Workout()
            {
                ProgramName = programName,
                WorkoutTime = workoutTime,
                Exercises = new List<Ianf.Fittrack.Services.Dto.Exercise>()
            };

            // Act
            var result = _workoutService.AddWorkout(newWorkout);

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
        public void TestGetNextWorkout()
        {
            // Assemble
            var timestamp = DateTime.Now;
            var newWorkout = GetSampleWorkout();
            var workout = newWorkout.ValidateDto().IfLeft(new Ianf.Fittrack.Services.Domain.Workout(
                ProgramName.CreateProgramName(programName).IfNone(new Ianf.Fittrack.Services.Domain.ProgramName()),
                ProgramType.MadCow,
                DateTime.Now,
                new List<Ianf.Fittrack.Services.Domain.Exercise>()
                ));
            var workoutTwo = workout with { WorkoutTime = timestamp.AddDays(1) };

            _workoutRepository.Setup(w => w.GetWorkoutsAfterDate(It.IsAny<DateTime>())).Returns(new List<Ianf.Fittrack.Services.Domain.Workout>
            {
                workout, workoutTwo
            });

            // Act
            var nextWorkout = _workoutService.GetWorkoutForDate(DateTime.Now);

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
        public void TestGetNextWorkoutReturnsNoneIfNoWorkouts()
        {
            // Assemble
            _workoutRepository.Setup(w => w.GetWorkoutsAfterDate(It.IsAny<DateTime>())).Returns(new List<Ianf.Fittrack.Services.Domain.Workout>
            {
            });

            // Act
            var nextWorkout = _workoutService.GetWorkoutForDate(DateTime.Now);

            // Assert
            nextWorkout.Match
            (
                None: () => Assert.True(true, ""),
                Some: (s) => Assert.False(true, $"Expected 'None' return. Got {s}.")
            );
        }

        [Fact]
        public void TestGetNextWorkoutReturnsNoneIfDatePassedInIsMaxValue()
        {
            // Assemble
            var timestamp = DateTime.Now;
            var newWorkout = GetSampleWorkout();
            var workout = newWorkout.ValidateDto().IfLeft(new Ianf.Fittrack.Services.Domain.Workout(
                ProgramName.CreateProgramName(programName).IfNone(new Ianf.Fittrack.Services.Domain.ProgramName()),
                ProgramType.MadCow,
                DateTime.Now,
                new List<Ianf.Fittrack.Services.Domain.Exercise>()
                ));
            var workoutTwo = workout with { WorkoutTime = timestamp.AddDays(2) };

            _workoutRepository.Setup(w => w.GetWorkoutsAfterDate(It.IsAny<DateTime>())).Returns(new List<Ianf.Fittrack.Services.Domain.Workout>
            {
                workout, workoutTwo
            });

            // Act
            var nextWorkout = _workoutService.GetWorkoutForDate(DateTime.MaxValue);

            // Assert
            nextWorkout.Match
            (
                None: () => Assert.True(true, ""),
                Some: (s) => Assert.False(true, $"Expected 'None' return. Got {s}.")
            );
        }

        [Fact]
        public void TestGetNextWorkoutReturnsNoneIfDatePassedInIsMinValue()
        {
            // Assemble
            var timestamp = DateTime.Now;
            var newWorkout = GetSampleWorkout();
            var workout = newWorkout.ValidateDto().IfLeft(new Ianf.Fittrack.Services.Domain.Workout(
                ProgramName.CreateProgramName(programName).IfNone(new Ianf.Fittrack.Services.Domain.ProgramName()),
                ProgramType.MadCow,
                DateTime.Now,
                new List<Ianf.Fittrack.Services.Domain.Exercise>()
                ));
            var workoutTwo = workout with { WorkoutTime = timestamp.AddDays(2) };

            _workoutRepository.Setup(w => w.GetWorkoutsAfterDate(It.IsAny<DateTime>())).Returns(new List<Ianf.Fittrack.Services.Domain.Workout>
            {
                workout, workoutTwo
            });

            // Act
            var nextWorkout = _workoutService.GetWorkoutForDate(DateTime.MinValue);

            // Assert
            nextWorkout.Match
            (
                None: () => Assert.True(true, ""),
                Some: (s) => Assert.False(true, $"Expected 'None' return. Got {s}.")
            );
        }
    }
}