#nullable disable
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System;
using System.Linq;
using Moq;
using Xunit;
using System.Threading.Tasks;
using Ianf.Fittrack.Services.Interfaces;
using LanguageExt;
using static LanguageExt.Prelude;
using System.Collections.Generic;
using Ianf.Fittrack.Services.Errors;
using Ianf.Fittrack.Services.Domain;

namespace Ianf.Fittrack.Services.Tests
{
    public class MatchEventServiceTests
    {
        private readonly Mock<IMatchEventRepository> _matchEventRepository;
        private readonly IMatchEventService _matchEventService;

        private readonly int userId = 1234;
        private readonly int matchId = 22;
        private DateTime eventTime = DateTime.Now;
        private readonly MatchEventType matchEventType = MatchEventType.Conversion;

        public Fittrack.Services.Dto.MatchEvent GetSampleMatchEvent() =>
            new Fittrack.Services.Dto.MatchEvent() 
            {
                UserId = userId,
                MatchId = matchId,
                EventTime = eventTime,
                MatchEventType = matchEventType
            };

        public MatchEventServiceTests()
        {
            _matchEventRepository = new Mock<IMatchEventRepository>();
            _matchEventService = new MatchEventService(_matchEventRepository.Object);
        }

        [Fact]
        public async void TestLoginWithValidUserid() 
        {
            // Assemble
            var userId = 42;

            // Act
            var result = await _matchEventService.LoginWithUserId(userId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async void TestLoginWithInvalidUserid() 
        {
            // Assemble
            var userId = -42;

            // Act
            var result = await _matchEventService.LoginWithUserId(userId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async void TestAddNewMatchEventAsyncSuccess()
        {
            // Assemble
            var newMatchEvent = GetSampleMatchEvent();
            Either<IEnumerable<Error>, int> returnValue = Right(1);
            _matchEventRepository
                .Setup(w => w.AddNewMatchEventAsync(It.IsAny<Fittrack.Services.Domain.MatchEvent>()))
                .Returns(Task.FromResult(returnValue));

            // Act
            var result = await _matchEventService.AddNewMatchEventAsync(newMatchEvent);

            // Assert
            _matchEventRepository.Verify(w => w.AddNewMatchEventAsync(It.IsAny<Fittrack.Services.Domain.MatchEvent>()));
            result.Match(
                Left: (err) => Assert.False(true, "Expected no errors to be returned."),
                Right: (newId) => Assert.Equal(1, newId)
            );
        }

        [Fact]
        public async void TestAddNewMatchEventAsyncUserIdInvalid()
        {
            // Assemble
            var newMatchEvent = GetSampleMatchEvent();
            newMatchEvent.UserId = -42;

            // Act
            var result = await _matchEventService.AddNewMatchEventAsync(newMatchEvent);

            // Assert
            result.Match(
                Left: (err) => {
                    var dtoError = (DtoValidationError)err.First();
                    Assert.Equal("MatchEvent", dtoError.DtoType);
                    Assert.Equal("UserId", dtoError.DtoProperty);
                },
                Right: (newId) => Assert.False(true, "Expected error.")
            );
        }

        [Fact]
        public async void TestAddNewMatchEventAsyncMatchIdInvalid()
        {
            // Assemble
            var newMatchEvent = GetSampleMatchEvent();
            newMatchEvent.MatchId = -42;

            // Act
            var result = await _matchEventService.AddNewMatchEventAsync(newMatchEvent);

            // Assert
            result.Match(
                Left: (err) => {
                    var dtoError = (DtoValidationError)err.First();
                    Assert.Equal("MatchEvent", dtoError.DtoType);
                    Assert.Equal("MatchId", dtoError.DtoProperty);
                },
                Right: (newId) => Assert.False(true, "Expected error.")
            );
        }

        [Fact]
        public async void TestAddNewMatchEventAsyncSqlError()
        {
            // Assemble
            var newMatchEvent = GetSampleMatchEvent();
            var errorMessage = "Sql Connection Exception";
            List<Error> errors = new List<Error> { new SqlError(errorMessage) };
            Either<IEnumerable<Error>, int> returnValue = errors;
            _matchEventRepository
                .Setup(w => w.AddNewMatchEventAsync(It.IsAny<Fittrack.Services.Domain.MatchEvent>()))
                .Returns(Task.FromResult(returnValue));

            // Act
            var result = await _matchEventService.AddNewMatchEventAsync(newMatchEvent);

            // Assert
            result.Match(
                Left: (err) => {
                    var sqlError = (SqlError)err.First();
                    Assert.Equal(errorMessage, sqlError.ErrorMessage);
                },
                Right: (newId) => Assert.False(true, "Expected error.")
            );
        }  
    
        [Fact]
        public async void TestGetAllEventsByUserId()
        {
            // Assemble
            var matchEvents = new List<MatchEvent>(){
                new MatchEvent(
                    UserId.CreateUserId(42).IfNone(new UserId()),
                    MatchId.CreateMatchId(22).IfNone(new MatchId()),
                    DateTime.Now.AddDays(-1),
                    MatchEventType.Conversion
                ),
                new MatchEvent(
                    UserId.CreateUserId(42).IfNone(new UserId()),
                    MatchId.CreateMatchId(22).IfNone(new MatchId()),
                    DateTime.Now.AddDays(-1),
                    MatchEventType.Conversion
                )
            };
            Either<IEnumerable<Error>, List<MatchEvent>> returnValue = Right(matchEvents);
            _matchEventRepository.Setup(m => m.GetAllMatchEventsByUserIdAsync(It.IsAny<UserId>(), It.IsAny<MatchId>())).Returns(Task.FromResult(returnValue));

            // Act
            var result = await _matchEventService.GetAllMatchEventsByUserIdAsync(42, 22);

            // Assert
            _matchEventRepository.Verify(w => w.GetAllMatchEventsByUserIdAsync(It.IsAny<UserId>(), It.IsAny<MatchId>()));
            result.Match(
                Left: (err) => Assert.False(true, "Expected no errors to be returned."),
                Right: (matchEvents) => Assert.Equal(2, matchEvents.Count)
            );
        }

        [Fact]
        public async void TestGetAllEventsByUserIdWithInvalidUserId()
        {
            // Assemble

            // Act
            var result = await _matchEventService.GetAllMatchEventsByUserIdAsync(-42, 22);

            // Assert
            result.Match(
                Left: (err) => {
                    var dtoError = (DtoValidationError)err.First();
                    Assert.Equal("MatchEvent", dtoError.DtoType);
                    Assert.Equal("UserId", dtoError.DtoProperty);
                },
                Right: (newId) => Assert.False(true, "Expected error.")
            );
        }

        [Fact]
        public async void TestGetAllEventsByUserIdWithInvalidMatchId()
        {
            // Assemble

            // Act
            var result = await _matchEventService.GetAllMatchEventsByUserIdAsync(42, -22);

            // Assert
            result.Match(
                Left: (err) => {
                    var dtoError = (DtoValidationError)err.First();
                    Assert.Equal("MatchEvent", dtoError.DtoType);
                    Assert.Equal("MatchId", dtoError.DtoProperty);
                },
                Right: (newId) => Assert.False(true, "Expected error.")
            );
        }
    }
}