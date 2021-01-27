using System.Text.Json;
using System.Text.Json.Serialization;
using Ianf.Fittrack.Services.Domain;
using Xunit;

namespace Ianf.Fittrack.Services.Tests.Domain
{
    public class NonNegativeIntTests
    {
        [Fact]
        public void TestCreateNonNegativeInt()
        {
            // Assemble
            var testValue = 1;
 
            // Act
            var newItem = NonNegativeInt.CreateNonNegativeInt(testValue);
 
            // Assert
            newItem
                .Match(
                    None: () => Assert.True(false, "Expected successful creation."),
                    Some: (s) => Assert.Equal(testValue, s.Value)
                );
        }
 
        [Fact]
        public void TestCreateNonNegativeIntZeroSuccess()
        {
            // Assemble
            var testValue = 0;
 
            // Act
            var newItem = NonNegativeInt.CreateNonNegativeInt(testValue);
 
            // Assert
            newItem
                .Match(
                    None: () => Assert.True(false, "Expected successful creation."),
                    Some: (s) => Assert.Equal(testValue, s.Value)
                );
        }
 
        [Fact]
        public void TestCreateNonNegativeIntNegativeFail()
        {
            // Assemble
            var testValue = -42;
 
            // Act
            var newItem = NonNegativeInt.CreateNonNegativeInt(testValue);
 
            // Assert
            newItem
                .Match(
                    None: () => Assert.True(true),
                    Some: (s) => Assert.True(false, "Expected no item to be created.")
                );
        }
 
        [Fact]
        public void TestCreateNonNegativeIntEqual()
        {
            // Assemble
            var testFirstValue = 42;
            var testSecondValue = 42;
 
            // Act
            var testFirstItem = NonNegativeInt.CreateNonNegativeInt(testFirstValue);
            var testSecondItem = NonNegativeInt.CreateNonNegativeInt(testSecondValue);
            var result = testFirstItem.Equals(testSecondItem);
 
            // Assert
            Assert.True(result);
        }
 
        [Fact]
        public void TestCreateNonNegativeIntNotEqual()
        {
            // Assemble
            var testFirstValue = 41;
            var testSecondValue = 42;
 
            // Act
            var testFirstItem = NonNegativeInt.CreateNonNegativeInt(testFirstValue);
            var testSecondItem = NonNegativeInt.CreateNonNegativeInt(testSecondValue);
            var result = testFirstItem.Equals(testSecondItem);
 
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void TestSerialisation()
        {
            // Assemble
            var testFirstItem = NonNegativeInt.CreateNonNegativeInt(42).IfNone(new NonNegativeInt());
            var serialisedData = JsonSerializer.Serialize(testFirstItem);

            // Act
            var deserialisedItem = JsonSerializer.Deserialize<NonNegativeInt>(serialisedData);

            // Assert
            Assert.Equal(42, deserialisedItem.Value);
        }
    }
}
