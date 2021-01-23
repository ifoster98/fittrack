using System;

namespace Ianf.Fittrack.Repositories.Entities
{
    public partial class MatchEvent
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MatchId { get; set; }
        public DateTime EventTime { get; set; }
        public int MatchEventType { get; set; }
    }
}
