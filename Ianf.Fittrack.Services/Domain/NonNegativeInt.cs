using LanguageExt;
using System;
using static LanguageExt.Prelude;

namespace Ianf.Fittrack.Services.Domain
{
    public struct NonNegativeInt : IEquatable<NonNegativeInt>
    {
        public int Value { get; }

        private NonNegativeInt(int nonNegativeInt) => Value = nonNegativeInt;

        public static Option<NonNegativeInt> CreateNonNegativeInt(int nonNegativeInt) =>
            IsValid(nonNegativeInt)
                ? Some(new NonNegativeInt(nonNegativeInt))
                : None;

        private static bool IsValid(int nonNegativeInt) => nonNegativeInt >= 0;

        public bool Equals(NonNegativeInt other) => Value == other.Value;
    }
}