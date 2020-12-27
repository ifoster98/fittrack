using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;

namespace Ianf.Fittrack.Workouts.Domain 
{
    public record DtoValidationError(string errorMessage, string DtoType, string DtoProperty) : Error(errorMessage) { };
}
