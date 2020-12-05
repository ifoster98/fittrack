#nullable disable // Explicitly allow nulls for testing of types.
using System;
using System.Collections.Generic;
using Ianf.Fittrack.Workouts.Domain;
using Xunit;

namespace Ianf.Fittrack.UnitTest.Workouts.Domain
{
    public class ProgramNameTests
    {
        [Fact]
        public void TestCreateProgramName()
        {
            // Assemble
            var testValue = "Test";
 
            // Act
            var newItem = ProgramName.CreateProgramName(testValue);
 
            // Assert
            newItem
                .Match(
                    None: () => Assert.True(false, "Expected successful creation."),
                    Some: (s) => Assert.Equal(testValue, s.Value)
                );
        }
 
        [Fact]
        public void TestCreateProgramNameFailNull()
        {
            // Assemble
 
            // Act
            var newItem = ProgramName.CreateProgramName(null);
 
            // Assert
            newItem
                .Match(
                    None: () => Assert.True(true),
                    Some: (s) => Assert.True(false, "Expected no item to be created.")
                );
        }
 
        [Fact]
        public void TestCreateProgramNameFailEmpty()
        {
            // Assemble
            var testValue = string.Empty;
 
            // Act
            var newItem = ProgramName.CreateProgramName(testValue);
 
            // Assert
            newItem
                .Match(
                    None: () => Assert.True(true),
                    Some: (s) => Assert.True(false, "Expected no item to be created.")
                );
        }
 
        [Fact]
        public void TestCreateProgramNameEqual()
        {
            // Assemble
            var testFirstValue = "Test";
            var testSecondValue = "Test";
 
            // Act
            var testFirstItem = ProgramName.CreateProgramName(testFirstValue);
            var testSecondItem = ProgramName.CreateProgramName(testSecondValue);
            var result = testFirstItem.Equals(testSecondItem);
 
            // Assert
            Assert.True(result);
        }
 
        [Fact]
        public void TestCreateProgramNameNotEqual()
        {
            // Assemble
            var testFirstValue = "Test";
            var testSecondValue = "TestTwo";
 
            // Act
            var testFirstItem = ProgramName.CreateProgramName(testFirstValue);
            var testSecondItem = ProgramName.CreateProgramName(testSecondValue);
            var result = testFirstItem.Equals(testSecondItem);
 
            // Assert
            Assert.False(result);
        }
    }
}
