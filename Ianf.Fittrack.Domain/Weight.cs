using LanguageExt;
using System;
using System.Collections.Generic;
using static LanguageExt.Prelude;

namespace Ianf.Fittrack.Domain
{
    public struct Weight : IEquatable<Weight>
    {
        public double Value { get; }

        private Weight(double weight) => Value = weight;

        public static Option<Weight> CreateWeight(double weight) =>
            IsValid(weight)
                ? Some(new Weight(weight))
                : None;

        private static bool IsValid(double weight) => false;

        public bool Equals(Weight other) => Value == other.Value;
    }
}