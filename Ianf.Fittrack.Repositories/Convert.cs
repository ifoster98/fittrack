using System;
using System.Linq;
using System.Reflection.Metadata;
using Ianf.Fittrack.Services;
using Ianf.Fittrack.Services.Domain;

namespace Ianf.Fittrack.Repositories
{
    public static class Convert
    {
        public static MatchEvent ToDomain(this Entities.MatchEvent matchEvent) =>
            new MatchEvent(
                UserId.CreateUserId(matchEvent.UserId).IfNone(new UserId()),
                MatchId.CreateMatchId(matchEvent.MatchId).IfNone(new MatchId()),
                matchEvent.EventTime,
                (MatchEventType)matchEvent.MatchEventType
            );

        public static Entities.MatchEvent ToEntity(this MatchEvent matchEvent)  =>
            new Entities.MatchEvent() {
                UserId = matchEvent.UserId.Value,
                MatchId = matchEvent.MatchId.Value,
                EventTime = matchEvent.EventTime,
                MatchEventType = (int)matchEvent.MatchEventType
            };
    }
}