using System.Text.Json;
using System.Text.Json.Serialization;
using Ianf.Fittrack.Services.Domain;
using Xunit;

namespace Ianf.Fittrack.Services.Tests.Domain
{
    public class WeightTests
    {
        [Fact]
        public void TestCreateWeight()
        {
            // Assemble
            var testValue = 2.5M; 
 
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
        public void TestCreateWeightFailNegative()
        {
            // Assemble
            var testValue = -2.5M; 
 
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
        public void TestCreateWeightFail()
        {
            // Assemble
            var testValue = 3.456M;
 
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
            var testFirstValue = 1.25M;
            var testSecondValue = 1.25M;
 
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
            var testFirstValue = 5M;
            var testSecondValue = 7.5M;
 
            // Act
            var testFirstItem = Weight.CreateWeight(testFirstValue);
            var testSecondItem = Weight.CreateWeight(testSecondValue);
            var result = testFirstItem.Equals(testSecondItem);
 
            // Assert
            Assert.False(result);
        }
   
        [Fact]
        public void TestSerialisation()
        {
            // Assemble
            var testFirstItem = Weight.CreateWeight(42).IfNone(new Weight());
            var serialisedData = JsonSerializer.Serialize(testFirstItem);

            // Act
            var deserialisedItem = JsonSerializer.Deserialize<Weight>(serialisedData);

            // Assert
            Assert.Equal(42, deserialisedItem.Value);
        }
    }
}
