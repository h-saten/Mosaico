using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Base.Extensions;
using Mosaico.Domain.ProjectManagement.Abstractions;

namespace Mosaico.Application.ProjectManagement.Queries.ProjectPreValidation
{
    public class ProjectPreValidationQueryHandler : IRequestHandler<ProjectPreValidationQuery, ProjectPreValidationQueryResponse>
    {
        private readonly IProjectDbContext _context;

        public ProjectPreValidationQueryHandler(IProjectDbContext context)
        {
            _context = context;
        }

        public async Task<ProjectPreValidationQueryResponse> Handle(ProjectPreValidationQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                return new ProjectPreValidationQueryResponse();
            }
            
            var comparableTitle = request.Title.Trim().ToUpperInvariant();
            var comparableSlug = request.Title.Trim().ToSlug();
            
            var titleExists = await _context.Projects.AnyAsync(p => p.TitleInvariant == comparableTitle, cancellationToken);
            var slugExists =
                await _context.Projects.AnyAsync(p => p.SlugInvariant == comparableSlug, cancellationToken);

            return new ProjectPreValidationQueryResponse
            {
                IsTitleValid = !titleExists && !slugExists
            };
        }
    }
}