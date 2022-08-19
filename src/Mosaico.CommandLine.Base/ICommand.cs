using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mosaico.CommandLine.Base
{
    public interface ICommand
    {
        List<Option> Options { get; set; }
        Task Execute();
    }
}