using System;

namespace Mosaico.CommandLine.Base
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandAttribute : Attribute
    {
        public CommandAttribute(string name, string description = "")
        {
            Name = name;
            Description = description;
        }

        public string Description { get; set; }
        public string Name { get; set; }
    }
}