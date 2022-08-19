using System;
using MediatR;
using Mosaico.Application.ProjectManagement.DTOs.TokenPage;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.ProjectManagement.Queries.TokenPage.GetTokenPage
{
    // [Cache("{{Id}}_{{Language}}")]
    public class GetTokenPageQuery : IRequest<PageDTO>
    {
        public Guid Id { get; set; }
        public string Language { get; set; }
    }
}