using LanguageExt;
using System;
using System.Collections.Generic;
using static LanguageExt.Prelude;

namespace Ianf.Fittrack.Workouts.Domain
{
    public struct Weight : IEquatable<Weight>
    {
        public double Value { get; }

        private Weight(double weight) => Value = weight;

        public static Option<Weight> CreateWeight(double weight) =>
            IsValid(weight)
                ? Some(new Weight(weight))
                : None;

        private static bool IsValid(double weight) => weight > 0 && (weight % 0.5) == 0;

        public bool Equals(Weight other) => Value == other.Value;
    }
}