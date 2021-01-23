using LanguageExt;
using System;
using static LanguageExt.Prelude;

namespace Ianf.Fittrack.Services.Domain
{
    public struct MatchId : IEquatable<MatchId>
    {
        public int Value { get; }

        private MatchId(int matchId) => Value = matchId;

        public static Option<MatchId> CreateMatchId(int matchId) =>
            IsValid(matchId)
                ? Some(new MatchId(matchId))
                : None;

        private static bool IsValid(int matchId) => matchId > 0;

        public bool Equals(MatchId other) => Value == other.Value;
    }
}