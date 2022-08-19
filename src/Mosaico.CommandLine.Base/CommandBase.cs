using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mosaico.CommandLine.Base
{
    public abstract class CommandBase : ICommand
    {
        public List<Option> Options { get; set; } = new();
        public abstract Task Execute();

        protected void SetOption(string pattern, string description, Action<string> valueSetter)
        {
            Options.Add(new Option
            {
                Description = description,
                Pattern = pattern,
                Value = valueSetter
            });
        }
    }
}