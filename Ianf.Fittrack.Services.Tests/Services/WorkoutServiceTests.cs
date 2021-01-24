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

        public Ianf.Fittrack.Services.Dto.PlannedWorkout GetSamplePlannedWorkout() =>
            new Ianf.Fittrack.Services.Dto.PlannedWorkout() 
            {
                ProgramName = programName,
                WorkoutTime = workoutTime,
                Exercises = new List<Ianf.Fittrack.Services.Dto.Exercise>()
                {
                    new Ianf.Fittrack.Services.Dto.Exercise()
                    {
                        ExerciseType = Ianf.Fittrack.Services.Dto.ExerciseType.Deadlift,
                        Order = 1,
                        ExerciseEntries = new List<Ianf.Fittrack.Services.Dto.ExerciseEntry>()
                        {
                            new Ianf.Fittrack.Services.Dto.ExerciseEntry()
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
        public void TestAddNewWorkoutSuccess()
        {
            // Assemble
            var newWorkout = GetSamplePlannedWorkout();
            _workoutRepository
                .Setup(w => w.AddWorkout(It.IsAny<Ianf.Fittrack.Services.Domain.PlannedWorkout>()))
                .Returns(Ianf.Fittrack.Services.Domain.PositiveInt.CreatePositiveInt(1).IfNone(new Ianf.Fittrack.Services.Domain.PositiveInt()));

            // Act
            var result = _workoutService.AddNewWorkout(newWorkout);

            // Assert
            _workoutRepository.Verify(w => w.AddWorkout(It.IsAny<Ianf.Fittrack.Services.Domain.PlannedWorkout>()));
            result.Match(
                Left: (err) => Assert.False(true, "Expected no errors to be returned."),
                Right: (newId) => Assert.Equal(1, newId.Value)
            );
        }

        [Fact]
        public void TestAddNewWorkoutFailsIfProgramNameIsEmpty()
        {
            // Assemble
            var newWorkout = GetSamplePlannedWorkout();
            newWorkout.ProgramName = string.Empty;

            // Act
            var result = _workoutService.AddNewWorkout(newWorkout);

            // Assert
            result.Match(
                Left: (err) => {
                    Assert.Equal("PlannedWorkout", err.First().DtoType);
                    Assert.Equal("ProgramName", err.First().DtoProperty);
                },
                Right: (newId) => Assert.False(true, "Expected error.")
            );
        }

        [Fact]
        public void TestAddNewWorkoutFailsIfProgramNameAndDateIsDuplicate()
        {
            // Assemble
            var newWorkout = GetSamplePlannedWorkout();
            _workoutRepository.Setup(w => w.HasWorkout(It.IsAny<DateTime>(), It.IsAny<Ianf.Fittrack.Services.Dto.ProgramType>(), It.IsAny<Ianf.Fittrack.Services.Domain.ProgramName>())).Returns(true);

            // Act
            var result = _workoutService.AddNewWorkout(newWorkout);

            // Assert
            result.Match(
                Left: (err) => {
                    Assert.Equal("Duplicate workout definition.", err.First().ErrorMessage);
                    Assert.Equal("PlannedWorkout", err.First().DtoType);
                },
                Right: (newId) => Assert.False(true, "Expected error.")
            );
        }

        [Fact]
        public void TestAddNewWorkoutFailsIfExerciseOrderIsNotPositive()
        {
            // Assemble
            var newWorkout = GetSamplePlannedWorkout();
            var exercise = newWorkout.Exercises.First();
            exercise.Order = 0;
            newWorkout.Exercises.Clear();
            newWorkout.Exercises.Add(exercise);

            // Act
            var result = _workoutService.AddNewWorkout(newWorkout);

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
        public void TestAddNewWorkoutFailsIfExerciseEntryRepsIsNotPositive()
        {
            // Assemble
            var newWorkout = GetSamplePlannedWorkout();
            var exercise = newWorkout.Exercises.First();
            var set = exercise.ExerciseEntries.First();
            set.Reps = -2;
            exercise.ExerciseEntries.Clear();
            exercise.ExerciseEntries.Add(set);

            // Act
            var result = _workoutService.AddNewWorkout(newWorkout);

            // Assert
            result.Match(
                Left: (err) => {
                    Assert.Equal("ExerciseEntry", err.First().DtoType);
                    Assert.Equal("Reps", err.First().DtoProperty);
                },
                Right: (newId) => Assert.False(true, "Expected error.")
            );
        }

        [Fact]
        public void TestAddNewWorkoutFailsIfExerciseEntryWeightIsNotValid()
        {
            // Assemble
            var newWorkout = GetSamplePlannedWorkout();
            var exercise = newWorkout.Exercises.First();
            var set = exercise.ExerciseEntries.First();
            set.Weight = 130.1234M;
            exercise.ExerciseEntries.Clear();
            exercise.ExerciseEntries.Add(set);

            // Act
            var result = _workoutService.AddNewWorkout(newWorkout);

            // Assert
            result.Match(
                Left: (err) => {
                    Assert.Equal("ExerciseEntry", err.First().DtoType);
                    Assert.Equal("Weight", err.First().DtoProperty);
                },
                Right: (newId) => Assert.False(true, "Expected error.")
            );
        }

        [Fact]
        public void TestAddNewWorkoutFailsIfExerciseEntryOrderIsNotPositive()
        {
            // Assemble
            var newWorkout = GetSamplePlannedWorkout();
            var exercise = newWorkout.Exercises.First();
            var set = exercise.ExerciseEntries.First();
            set.Order = -4;
            exercise.ExerciseEntries.Clear();
            exercise.ExerciseEntries.Add(set);

            // Act
            var result = _workoutService.AddNewWorkout(newWorkout);

            // Assert
            result.Match(
                Left: (err) => {
                    Assert.Equal("ExerciseEntry", err.First().DtoType);
                    Assert.Equal("Order", err.First().DtoProperty);
                },
                Right: (newId) => Assert.False(true, "Expected error.")
            );
        }

        [Fact]
        public void TestAddNewWorkoutFailsIfExercisesListIsNull()
        {
            // Assemble
            var newWorkout = new Ianf.Fittrack.Services.Dto.PlannedWorkout()
            {
                ProgramName = programName,
                WorkoutTime = workoutTime,
                Exercises = null
            };

            // Act
            var result = _workoutService.AddNewWorkout(newWorkout);

            // Assert
            result.Match(
                Left: (err) => {
                    Assert.Equal("PlannedWorkout", err.First().DtoType);
                    Assert.Equal("Exercises", err.First().DtoProperty);
                },
                Right: (newId) => Assert.False(true, "Expected error.")
            );
        }

        [Fact]
        public void TestAddNewWorkoutFailsIfExercisesListHasNoExercises()
        {
            // Assemble
            var newWorkout = new Ianf.Fittrack.Services.Dto.PlannedWorkout()
            {
                ProgramName = programName,
                WorkoutTime = workoutTime,
                Exercises = new List<Ianf.Fittrack.Services.Dto.Exercise>()
            };

            // Act
            var result = _workoutService.AddNewWorkout(newWorkout);

            // Assert
            result.Match(
                Left: (err) => {
                    Assert.Equal("PlannedWorkout", err.First().DtoType);
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
            var newWorkout = GetSamplePlannedWorkout();
            newWorkout.WorkoutTime = timestamp.AddDays(1);
            var workout = newWorkout.ValidateDto().IfLeft(new Ianf.Fittrack.Services.Domain.PlannedWorkout(
                ProgramName.CreateProgramName(programName).IfNone(new Ianf.Fittrack.Services.Domain.ProgramName()),
                ProgramType.MadCow,
                DateTime.Now,
                new List<Ianf.Fittrack.Services.Domain.Exercise>()
                ));
            var workoutTwo = workout with { WorkoutTime = timestamp.AddDays(2) };

            _workoutRepository.Setup(w => w.GetPlannedWorkoutsAfterDate(It.IsAny<DateTime>())).Returns(new List<Ianf.Fittrack.Services.Domain.PlannedWorkout>
            {
                workout, workoutTwo
            });

            // Act
            var nextWorkout = _workoutService.GetNextWorkout(DateTime.Now, programName);

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
            _workoutRepository.Setup(w => w.GetPlannedWorkoutsAfterDate(It.IsAny<DateTime>())).Returns(new List<Ianf.Fittrack.Services.Domain.PlannedWorkout>
            {
            });

            // Act
            var nextWorkout = _workoutService.GetNextWorkout(DateTime.Now, programName);

            // Assert
            nextWorkout.Match
            (
                None: () => Assert.True(true, ""),
                Some: (s) => Assert.False(true, $"Expected 'None' return. Got {s}.")
            );
        }

        [Fact]
        public void TestGetNextWorkoutReturnsNoneIfDifferentProgramName()
        {
            // Assemble
            var timestamp = DateTime.Now;
            var newWorkout = GetSamplePlannedWorkout();
            newWorkout.WorkoutTime = timestamp.AddDays(1);
            var workout = newWorkout.ValidateDto().IfLeft(new Ianf.Fittrack.Services.Domain.PlannedWorkout(
                ProgramName.CreateProgramName(programName).IfNone(new Ianf.Fittrack.Services.Domain.ProgramName()),
                ProgramType.MadCow,
                DateTime.Now,
                new List<Ianf.Fittrack.Services.Domain.Exercise>()
                ));
            var workoutTwo = workout with { WorkoutTime = timestamp.AddDays(2) };

            _workoutRepository.Setup(w => w.GetPlannedWorkoutsAfterDate(It.IsAny<DateTime>())).Returns(new List<Ianf.Fittrack.Services.Domain.PlannedWorkout>
            {
                workout, workoutTwo
            });

            // Act
            var nextWorkout = _workoutService.GetNextWorkout(DateTime.Now, "Diffferent Program");

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
            var newWorkout = GetSamplePlannedWorkout();
            newWorkout.WorkoutTime = timestamp.AddDays(1);
            var workout = newWorkout.ValidateDto().IfLeft(new Ianf.Fittrack.Services.Domain.PlannedWorkout(
                ProgramName.CreateProgramName(programName).IfNone(new Ianf.Fittrack.Services.Domain.ProgramName()),
                ProgramType.MadCow,
                DateTime.Now,
                new List<Ianf.Fittrack.Services.Domain.Exercise>()
                ));
            var workoutTwo = workout with { WorkoutTime = timestamp.AddDays(2) };

            _workoutRepository.Setup(w => w.GetPlannedWorkoutsAfterDate(It.IsAny<DateTime>())).Returns(new List<Ianf.Fittrack.Services.Domain.PlannedWorkout>
            {
                workout, workoutTwo
            });

            // Act
            var nextWorkout = _workoutService.GetNextWorkout(DateTime.MaxValue, programName);

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
            var newWorkout = GetSamplePlannedWorkout();
            newWorkout.WorkoutTime = timestamp.AddDays(1);
            var workout = newWorkout.ValidateDto().IfLeft(new Ianf.Fittrack.Services.Domain.PlannedWorkout(
                ProgramName.CreateProgramName(programName).IfNone(new Ianf.Fittrack.Services.Domain.ProgramName()),
                ProgramType.MadCow,
                DateTime.Now,
                new List<Ianf.Fittrack.Services.Domain.Exercise>()
                ));
            var workoutTwo = workout with { WorkoutTime = timestamp.AddDays(2) };

            _workoutRepository.Setup(w => w.GetPlannedWorkoutsAfterDate(It.IsAny<DateTime>())).Returns(new List<Ianf.Fittrack.Services.Domain.PlannedWorkout>
            {
                workout, workoutTwo
            });

            // Act
            var nextWorkout = _workoutService.GetNextWorkout(DateTime.MinValue, programName);

            // Assert
            nextWorkout.Match
            (
                None: () => Assert.True(true, ""),
                Some: (s) => Assert.False(true, $"Expected 'None' return. Got {s}.")
            );
        }
    }
}