using System.Collections.Generic;

namespace Ianf.Fittrack.Dto
{
    public struct Exercise 
    {
        public List<Set> Sets { get; set; }
        public int Order { get; set; }
    }
}