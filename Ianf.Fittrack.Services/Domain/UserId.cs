using LanguageExt;
using System;
using static LanguageExt.Prelude;

namespace Ianf.Fittrack.Services.Domain
{
    public struct UserId : IEquatable<UserId>
    {
        public int Value { get; }

        private UserId(int userId) => Value = userId;

        public static Option<UserId> CreateUserId(int userId) =>
            IsValid(userId)
                ? Some(new UserId(userId))
                : None;

        private static bool IsValid(int userId) => userId > 0;

        public bool Equals(UserId other) => Value == other.Value;
    }
}