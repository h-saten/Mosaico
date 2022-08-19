using MediatR;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.ProjectManagement.Queries.GetTemplateContent
{
    // [Cache("{{Key}}_{{Language}}")]
    public class GetTemplateContentQuery : IRequest<GetTemplateContentQueryResponse>
    {
        public string Key { get; set; }
        public string Language { get; set; }

    }
}