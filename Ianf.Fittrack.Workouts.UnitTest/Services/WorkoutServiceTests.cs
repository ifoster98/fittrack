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
                        Order = 1,
                        Sets = new List<Dto.Set>()
                        {
                            new Dto.Set()
                            {
                                ExerciseType = ExerciseType.Deadlift,
                                Reps = 5,
                                Weight = 130,
                                Order = 1
                            }
                        }
                    }
                },
                ActualExercises = new List<Dto.Exercise>()
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
                        Order = 1,
                        Sets = new List<Dto.Set>()
                        {
                            new Dto.Set()
                            {
                                ExerciseType = ExerciseType.Deadlift,
                                Reps = 5,
                                Weight = 130,
                                Order = 1
                            }
                        }
                    }
                },
                ActualExercises = new List<Dto.Exercise>()
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
                        Order = 0,
                        Sets = new List<Dto.Set>()
                        {
                            new Dto.Set()
                            {
                                ExerciseType = ExerciseType.Deadlift,
                                Reps = 5,
                                Weight = 130,
                                Order = 1
                            }
                        }
                    }
                },
                ActualExercises = new List<Dto.Exercise>()
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
                        Order = 1,
                        Sets = new List<Dto.Set>()
                        {
                            new Dto.Set()
                            {
                                ExerciseType = ExerciseType.Deadlift,
                                Reps = -2,
                                Weight = 130,
                                Order = 1
                            }
                        }
                    }
                },
                ActualExercises = new List<Dto.Exercise>()
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
                        Order = 1,
                        Sets = new List<Dto.Set>()
                        {
                            new Dto.Set()
                            {
                                ExerciseType = ExerciseType.Deadlift,
                                Reps = 5,
                                Weight = 130.1234,
                                Order = 1
                            }
                        }
                    }
                },
                ActualExercises = new List<Dto.Exercise>()
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
                        Order = 1,
                        Sets = new List<Dto.Set>()
                        {
                            new Dto.Set()
                            {
                                ExerciseType = ExerciseType.Deadlift,
                                Reps = 5,
                                Weight = 130,
                                Order = -4
                            }
                        }
                    }
                },
                ActualExercises = new List<Dto.Exercise>()
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
                PlannedExercises = null,
                ActualExercises = new List<Dto.Exercise>()
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
        public void TestAddNewWorkoutFailsIfActualExercisesListIsNull()
        {
            // Assemble
            var newWorkout = new Dto.Workout()
            {
                ProgramName = "Workout1",
                WorkoutTime = workoutTime,
                PlannedExercises = new List<Dto.Exercise>(),
                ActualExercises = null
            };

            // Act
            var result = _workoutService.AddNewWorkout(newWorkout);

            // Assert
            result.Match(
                Left: (err) => {
                    Assert.Equal("Workout", err.First().DtoType);
                    Assert.Equal("ActualExercises", err.First().DtoProperty);
                },
                Right: (newId) => Assert.False(true, "Expected error.")
            );
        }

        // Reject if workout has actual exercises
        [Fact]
        public void TestAddNewWorkoutFailsIfActualExercisesListHasExercises()
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
                        Order = 1,
                        Sets = new List<Dto.Set>()
                        {
                            new Dto.Set()
                            {
                                ExerciseType = ExerciseType.Deadlift,
                                Reps = 5,
                                Weight = 130,
                                Order = 1
                            }
                        }
                    }
                },
                ActualExercises = new List<Dto.Exercise>()
                {
                    new Dto.Exercise()
                    {
                        Order = 1,
                        Sets = new List<Dto.Set>()
                        {
                            new Dto.Set()
                            {
                                ExerciseType = ExerciseType.Deadlift,
                                Reps = 5,
                                Weight = 130,
                                Order = 1
                            }
                        }
                    }
                },
            };

            // Act
            var result = _workoutService.AddNewWorkout(newWorkout);

            // Assert
            result.Match(
                Left: (err) => {
                    Assert.Equal("Workout", err.First().DtoType);
                    Assert.Equal("ActualExercises", err.First().DtoProperty);
                },
                Right: (newId) => Assert.False(true, "Expected error.")
            );
        }

        // Reject if workout planned exercises is empty
        [Fact]
        public void TestAddNewWorkoutFailsIfPlannedExercisesListHasNoExercises()
        {
            // Assemble
            var newWorkout = new Dto.Workout()
            {
                ProgramName = "Workout1",
                WorkoutTime = workoutTime,
                PlannedExercises = new List<Dto.Exercise>(),
                ActualExercises = new List<Dto.Exercise>()
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
    }
}