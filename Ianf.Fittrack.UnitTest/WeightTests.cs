using System;
using System.Collections.Generic;
using Ianf.Fittrack.Domain;
using Xunit;

namespace Ianf.Fittrack.UnitTest.Domain
{
    public class WeightTests
    {
        [Fact]
        public void TestCreateWeight()
        {
            // Assemble
            var testValue = default(double);
 
            // Act
            var newItem = Weight.CreateWeight(testValue);
 
            // Assert
            newItem
                .Match(
                    None: () => Assert.True(false, "Expected successful creation."),
                    Some: (s) => Assert.Equal(testValue, s.Value)
                );
        }
 
        [Fact]
        public void TestCreateWeightFail()
        {
            // Assemble
            var testValue = default(double);
 
            // Act
            var newItem = Weight.CreateWeight(testValue);
 
            // Assert
            newItem
                .Match(
                    None: () => Assert.True(true),
                    Some: (s) => Assert.True(false, "Expected no item to be created.")
                );
        }
 
        [Fact]
        public void TestCreateWeightEqual()
        {
            // Assemble
            var testFirstValue = default(double);
            var testSecondValue = default(double);
 
            // Act
            var testFirstItem = Weight.CreateWeight(testFirstValue);
            var testSecondItem = Weight.CreateWeight(testSecondValue);
            var result = testFirstItem.Equals(testSecondItem);
 
            // Assert
            Assert.True(result);
        }
 
        [Fact]
        public void TestCreateWeightNotEqual()
        {
            // Assemble
            var testFirstValue = default(double);
            var testSecondValue = default(double);
 
            // Act
            var testFirstItem = Weight.CreateWeight(testFirstValue);
            var testSecondItem = Weight.CreateWeight(testSecondValue);
            var result = testFirstItem.Equals(testSecondItem);
 
            // Assert
            Assert.False(result);
        }
    }
}
