using Xunit;
using Ianf.Fittrack.Workouts.Dto;
using Ianf.Fittrack.Workouts.Domain;
using System.Collections.Generic;
using System;
using Ianf.Fittrack.Workouts.Generators.Interfaces;

namespace Ianf.Fittrack.Workouts.Generators.Tests
{
    public class UpperLowerGeneratorTests 
    {
        private readonly string _programName = "TestWorkout";
        private readonly IGenerator generator = new UpperLowerGenerator();

        private Domain.PlannedWorkout GetSamplePlannedWorkout() => 
            new Domain.PlannedWorkout(
                1,
                ProgramName.CreateProgramName(_programName).IfNone(new ProgramName()),
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
                            ),
                            new Domain.Set(
                                PositiveInt.CreatePositiveInt(5).IfNone(new PositiveInt()),
                                Weight.CreateWeight(130.0M).IfNone(new Weight()),
                                PositiveInt.CreatePositiveInt(2).IfNone(new PositiveInt()) 
                            ),
                            new Domain.Set(
                                PositiveInt.CreatePositiveInt(5).IfNone(new PositiveInt()),
                                Weight.CreateWeight(130.0M).IfNone(new Weight()),
                                PositiveInt.CreatePositiveInt(3).IfNone(new PositiveInt()) 
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
                ProgramName.CreateProgramName(_programName).IfNone(new ProgramName()),
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
                            ),
                            new Domain.Set(
                                PositiveInt.CreatePositiveInt(4).IfNone(new PositiveInt()),
                                Weight.CreateWeight(130.0M).IfNone(new Weight()),
                                PositiveInt.CreatePositiveInt(2).IfNone(new PositiveInt()) 
                            ),
                            new Domain.Set(
                                PositiveInt.CreatePositiveInt(3).IfNone(new PositiveInt()),
                                Weight.CreateWeight(130.0M).IfNone(new Weight()),
                                PositiveInt.CreatePositiveInt(3).IfNone(new PositiveInt()) 
                            )
                        },
                        PositiveInt.CreatePositiveInt(1).IfNone(new PositiveInt())
                    )
                }
            );
            return actualWorkout;
        }

        [Fact]
        public void TestGetProgramType()
        {
            // Assemble

            // Act
            var result = generator.GetProgramType();

            // Assert
            Assert.Equal(ProgramType.UpperLower, result);
        }

        [Fact]
        public void TestReturnPlannedWorkoutIfActualNotSuccessfull()
        {
            // Assemble
            var plannedWorkout = GetSamplePlannedWorkout();
            var actualWorkout = GetSampleActualWorkout();

            // Act
            var nextWorkout = generator.GetNextWorkout(plannedWorkout, actualWorkout);

            // Assert
            Assert.Equal(nextWorkout, plannedWorkout);
        }
    }
}