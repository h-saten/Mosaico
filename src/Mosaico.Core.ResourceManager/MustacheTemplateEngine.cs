using System.Collections.Generic;
using HandlebarsDotNet;
using Mosaico.Base.Abstractions;

namespace Mosaico.Core.ResourceManager
{
    public class MustacheTemplateEngine : ITemplateEngine
    {
        public string Build(string template, Dictionary<string, string> payload)
        {
            var compiledTemplate = Handlebars.Compile(template);
            return compiledTemplate(payload);
        }

        public string Build(string template, object payload)
        {
            var compiledTemplate = Handlebars.Compile(template);
            return compiledTemplate(payload);
        }
    }
}