using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mosaico.API.v1.DocumentManagement.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/documents")]
    [Route("api/v{version:apiVersion}/documents")]
    public partial class DocumentController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public DocumentController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }
    }
}
