using System.Collections.Generic;
using System.Linq;
using Ianf.Fittrack.Services.Errors;
using LanguageExt;

namespace Ianf.Fittrack.Services.Domain
{
    public static class Validator
    {
        public static Either<IEnumerable<Error>, MatchEvent> ValidateDto(this Dto.MatchEvent matchEvent)
        {
            var errors = new List<DtoValidationError>();
            var userId = new UserId();
            UserId.CreateUserId(matchEvent.UserId)
                .Match(
                    None: () => errors.Add(new DtoValidationError("Invalid amount for userId.", "MatchEvent", "UserId") ),
                    Some: (s) => userId = s
                );
            var matchId = new MatchId();
            MatchId.CreateMatchId(matchEvent.MatchId)
                .Match(
                    None: () => errors.Add(new DtoValidationError("Invalid amount for matchId.", "MatchEvent", "MatchId") ),
                    Some: (s) => matchId = s
                );
            if(errors.Any()) return errors;
            return new MatchEvent(userId, matchId, matchEvent.EventTime, matchEvent.MatchEventType);
        }
    }
}