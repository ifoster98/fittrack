using System;
using System.Collections.Generic;
using Ianf.Fittrack.Workouts.Domain;
using Ianf.Fittrack.Workouts.Persistance.Interfaces;
using Ianf.Fittrack.Workouts.Services;
using Ianf.Fittrack.Workouts.Services.Interfaces;
using Moq;
using Xunit;

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
    }
}