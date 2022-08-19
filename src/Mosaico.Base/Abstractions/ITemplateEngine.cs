using System.Collections.Generic;

namespace Mosaico.Base.Abstractions
{
    public interface ITemplateEngine
    {
        string Build(string template, Dictionary<string, string> payload);
        string Build(string template, object payload);
    }
}