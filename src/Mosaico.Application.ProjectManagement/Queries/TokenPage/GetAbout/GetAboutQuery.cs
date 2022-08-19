using System;
using MediatR;
using Mosaico.Application.ProjectManagement.DTOs;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.ProjectManagement.Queries.TokenPage.GetAbout
{
    // [Cache("{{PageId}}_{{Language}}")]
    public class GetAboutQuery : IRequest<AboutDTO>
    {
        public Guid PageId { get; set; }
        public string Language { get; set; }
    }
}