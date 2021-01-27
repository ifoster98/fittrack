using System.Text.Json;
using System.Text.Json.Serialization;
using Ianf.Fittrack.Services.Domain;
using Xunit;

namespace Ianf.Fittrack.Services.Tests.Domain
{
    public class PositiveIntTests
    {
        [Fact]
        public void TestCreatePositiveInt()
        {
            // Assemble
            var testValue = 1;
 
            // Act
            var newItem = PositiveInt.CreatePositiveInt(testValue);
 
            // Assert
            newItem
                .Match(
                    None: () => Assert.True(false, "Expected successful creation."),
                    Some: (s) => Assert.Equal(testValue, s.Value)
                );
        }
 
        [Fact]
        public void TestCreatePositiveIntZeroFail()
        {
            // Assemble
            var testValue = 0;
 
            // Act
            var newItem = PositiveInt.CreatePositiveInt(testValue);
 
            // Assert
            newItem
                .Match(
                    None: () => Assert.True(true),
                    Some: (s) => Assert.True(false, "Expected no item to be created.")
                );
        }
 
        [Fact]
        public void TestCreatePositiveIntNegativeFail()
        {
            // Assemble
            var testValue = -42;
 
            // Act
            var newItem = PositiveInt.CreatePositiveInt(testValue);
 
            // Assert
            newItem
                .Match(
                    None: () => Assert.True(true),
                    Some: (s) => Assert.True(false, "Expected no item to be created.")
                );
        }
 
        [Fact]
        public void TestCreatePositiveIntEqual()
        {
            // Assemble
            var testFirstValue = 42;
            var testSecondValue = 42;
 
            // Act
            var testFirstItem = PositiveInt.CreatePositiveInt(testFirstValue);
            var testSecondItem = PositiveInt.CreatePositiveInt(testSecondValue);
            var result = testFirstItem.Equals(testSecondItem);
 
            // Assert
            Assert.True(result);
        }
 
        [Fact]
        public void TestCreatePositiveIntNotEqual()
        {
            // Assemble
            var testFirstValue = 41;
            var testSecondValue = 42;
 
            // Act
            var testFirstItem = PositiveInt.CreatePositiveInt(testFirstValue);
            var testSecondItem = PositiveInt.CreatePositiveInt(testSecondValue);
            var result = testFirstItem.Equals(testSecondItem);
 
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void TestSerialisation()
        {
            // Assemble
            var testFirstItem = PositiveInt.CreatePositiveInt(42).IfNone(new PositiveInt());
            var serialisedData = JsonSerializer.Serialize(testFirstItem);

            // Act
            var deserialisedItem = JsonSerializer.Deserialize<PositiveInt>(serialisedData);

            // Assert
            Assert.Equal(42, deserialisedItem.Value);
        }
    }
}
