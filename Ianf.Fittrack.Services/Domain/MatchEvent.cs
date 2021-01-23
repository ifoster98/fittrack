using System;

namespace Ianf.Fittrack.Services.Domain
{
    public record MatchEvent(UserId UserId, MatchId MatchId, DateTime EventTime, MatchEventType MatchEventType)
    { 
        public Dto.MatchEvent ToDto()  =>
            new Dto.MatchEvent() {
                UserId = this.UserId.Value,
                MatchId = this.MatchId.Value,
                EventTime = this.EventTime,
                MatchEventType = this.MatchEventType
            };
    }
}
