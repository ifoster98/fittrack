using System;

namespace Ianf.Fittrack.Services.Dto
{
    public struct MatchEvent 
    {
        public int UserId { get; set; }
        public int MatchId { get; set; }
        public DateTime EventTime { get; set; }
        public MatchEventType MatchEventType { get; set; }
    }
}
