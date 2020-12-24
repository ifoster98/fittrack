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

namespace Ianf.Fittrack.UnitTest.Services
{
    public class WorkoutServiceTests
    {
        private readonly Mock<IWorkoutRepository> _workoutRepository;
        private readonly IWorkoutService _workoutService;

        private DateTime workoutTime = DateTime.Now;

        public WorkoutServiceTests()
        {
            _workoutRepository = new Mock<IWorkoutRepository>();
            _workoutService = new WorkoutService(_workoutRepository.Object);
        }

        [Fact]
        public void TestAddNewWorkoutSuccess()
        {
            // Assemble
            var newWorkout = new Dto.Workout() 
            {
                ProgramName = "Workout1",
                WorkoutTime = workoutTime,
                PlannedExercises = new List<Dto.Exercise>()
                {
                    new Dto.Exercise()
                    {
                        ExerciseType = ExerciseType.Deadlift,
                        Order = 1,
                        Sets = new List<Dto.Set>()
                        {
                            new Dto.Set()
                            {
                                Reps = 5,
                                Weight = 130,
                                Order = 1
                            }
                        }
                    }
                }
            };
            _workoutRepository
                .Setup(w => w.SaveWorkout(It.IsAny<Workout>()))
                .Returns(PositiveInt.CreatePositiveInt(1).IfNone(new PositiveInt()));

            // Act
            var result = _workoutService.AddNewWorkout(newWorkout);

            // Assert
            _workoutRepository.Verify(w => w.SaveWorkout(It.IsAny<Workout>()));
            result.Match(
                Left: (err) => Assert.False(true, "Expected no errors to be returned."),
                Right: (newId) => Assert.Equal(1, newId.Value)
            );
        }

        [Fact]
        public void TestAddNewWorkoutFailsIfProgramNameIsEmpty()
        {
            // Assemble
            var newWorkout = new Dto.Workout() 
            {
                ProgramName = string.Empty,
                WorkoutTime = workoutTime,
                PlannedExercises = new List<Dto.Exercise>()
                {
                    new Dto.Exercise()
                    {
                        ExerciseType = ExerciseType.Deadlift,
                        Order = 1,
                        Sets = new List<Dto.Set>()
                        {
                            new Dto.Set()
                            {
                                Reps = 5,
                                Weight = 130,
                                Order = 1
                            }
                        }
                    }
                }
            };

            // Act
            var result = _workoutService.AddNewWorkout(newWorkout);

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
        public void TestAddNewWorkoutFailsIfExerciseOrderIsNotPositive()
        {
            // Assemble
            var newWorkout = new Dto.Workout() 
            {
                ProgramName = "Workout1",
                WorkoutTime = workoutTime,
                PlannedExercises = new List<Dto.Exercise>()
                {
                    new Dto.Exercise()
                    {
                        ExerciseType = ExerciseType.Deadlift,
                        Order = 0,
                        Sets = new List<Dto.Set>()
                        {
                            new Dto.Set()
                            {
                                Reps = 5,
                                Weight = 130,
                                Order = 1
                            }
                        }
                    }
                }
            };

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
        public void TestAddNewWorkoutFailsIfSetRepsIsNotPositive()
        {
            // Assemble
            var newWorkout = new Dto.Workout() 
            {
                ProgramName = "Workout1",
                WorkoutTime = workoutTime,
                PlannedExercises = new List<Dto.Exercise>()
                {
                    new Dto.Exercise()
                    {
                        ExerciseType = ExerciseType.Deadlift,
                        Order = 1,
                        Sets = new List<Dto.Set>()
                        {
                            new Dto.Set()
                            {
                                Reps = -2,
                                Weight = 130,
                                Order = 1
                            }
                        }
                    }
                }
            };

            // Act
            var result = _workoutService.AddNewWorkout(newWorkout);

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
        public void TestAddNewWorkoutFailsIfSetWeightIsNotValid()
        {
            // Assemble
            var newWorkout = new Dto.Workout() 
            {
                ProgramName = "Workout1",
                WorkoutTime = workoutTime,
                PlannedExercises = new List<Dto.Exercise>()
                {
                    new Dto.Exercise()
                    {
                        ExerciseType = ExerciseType.Deadlift,
                        Order = 1,
                        Sets = new List<Dto.Set>()
                        {
                            new Dto.Set()
                            {
                                Reps = 5,
                                Weight = 130.1234,
                                Order = 1
                            }
                        }
                    }
                }
            };

            // Act
            var result = _workoutService.AddNewWorkout(newWorkout);

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
        public void TestAddNewWorkoutFailsIfSetOrderIsNotPositive()
        {
            // Assemble
            var newWorkout = new Dto.Workout() 
            {
                ProgramName = "Workout1",
                WorkoutTime = workoutTime,
                PlannedExercises = new List<Dto.Exercise>()
                {
                    new Dto.Exercise()
                    {
                        ExerciseType = ExerciseType.Deadlift,
                        Order = 1,
                        Sets = new List<Dto.Set>()
                        {
                            new Dto.Set()
                            {
                                Reps = 5,
                                Weight = 130,
                                Order = -4
                            }
                        }
                    }
                }
            };

            // Act
            var result = _workoutService.AddNewWorkout(newWorkout);

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
        public void TestAddNewWorkoutFailsIfPlannedExercisesListIsNull()
        {
            // Assemble
            var newWorkout = new Dto.Workout()
            {
                ProgramName = "Workout1",
                WorkoutTime = workoutTime,
                PlannedExercises = null
            };

            // Act
            var result = _workoutService.AddNewWorkout(newWorkout);

            // Assert
            result.Match(
                Left: (err) => {
                    Assert.Equal("Workout", err.First().DtoType);
                    Assert.Equal("PlannedExercises", err.First().DtoProperty);
                },
                Right: (newId) => Assert.False(true, "Expected error.")
            );
        }

        [Fact]
        public void TestAddNewWorkoutFailsIfPlannedExercisesListHasNoExercises()
        {
            // Assemble
            var newWorkout = new Dto.Workout()
            {
                ProgramName = "Workout1",
                WorkoutTime = workoutTime,
                PlannedExercises = new List<Dto.Exercise>()
            };

            // Act
            var result = _workoutService.AddNewWorkout(newWorkout);

            // Assert
            result.Match(
                Left: (err) => {
                    Assert.Equal("Workout", err.First().DtoType);
                    Assert.Equal("PlannedExercises", err.First().DtoProperty);
                },
                Right: (newId) => Assert.False(true, "Expected error.")
            );
        }

        [Fact]
        public void TestGetNextWorkout()
        {
            // Assemble
            var programName = ProgramName.CreateProgramName("Test Program").IfNone(new ProgramName());
            var workoutTime = DateTime.Now;
            var plannedExercises = new List<Exercise>();
            var w = new Workout(programName, workoutTime, plannedExercises);
            _workoutRepository.Setup(w => w.GetNextWorkout()).Returns(w);

            // Act
            var nextWorkout = _workoutService.GetNextWorkout();

            // Assert
            nextWorkout.Match(
                None: () => Assert.True(false, "Should have found a workout."),
                Some: (s) => Assert.Equal(s.ProgramName, w.ProgramName.Value)
            );
        }
    }
}