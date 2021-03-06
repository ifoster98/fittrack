using LanguageExt;
using System;
using static LanguageExt.Prelude;

namespace Ianf.Fittrack.Services.Domain
{
    public struct Weight : IEquatable<Weight>
    {
        public decimal Value { get; set; }

        private Weight(decimal weight) => Value = weight;

        public static Option<Weight> CreateWeight(decimal weight) =>
            IsValid(weight)
                ? Some(new Weight(weight))
                : None;

        private static bool IsValid(decimal weight) => weight >= 0 && (weight % 0.5M) == 0;

        public bool Equals(Weight other) => Value == other.Value;
    }
}