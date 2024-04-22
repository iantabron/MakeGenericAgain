using System.Collections.Generic;

namespace MakeGenericAgain
{
    public class Options
    {
        [FromCommandLine("f")]
        public string FileName { get; set; }

        [FromCommandLine("i")]
        public List<string> TypesToIgnore { get; set; } = [];
    }
}