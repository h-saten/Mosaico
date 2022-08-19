using System;

namespace Mosaico.CommandLine.Base
{
    public class Option
    {
        public string Pattern { get; set; }
        public string Description { get; set; }
        public Action<string> Value { get; set; }
    }
}