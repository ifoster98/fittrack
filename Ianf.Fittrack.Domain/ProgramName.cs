using LanguageExt;
using System;
using System.Collections.Generic;
using static LanguageExt.Prelude;

namespace Ianf.Fittrack.Domain
{
    public struct ProgramName : IEquatable<ProgramName>
    {
        public string Value { get; }

        private ProgramName(string programName) => Value = programName;

        public static Option<ProgramName> CreateProgramName(string programName) =>
            IsValid(programName)
                ? Some(new ProgramName(programName))
                : None;

        private static bool IsValid(string programName) => !String.IsNullOrEmpty(programName);

        public bool Equals(ProgramName other) => Value == other.Value;
    }
}