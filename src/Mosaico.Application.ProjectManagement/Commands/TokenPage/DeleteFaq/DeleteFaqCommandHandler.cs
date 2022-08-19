using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Serilog;

namespace Mosaico.Application.ProjectManagement.Commands.TokenPage.DeleteFaq
{
    public class DeleteFaqCommandHandler : IRequestHandler<DeleteFaqCommand>
    {
        private readonly IProjectDbContext _projectDbContext;
        private readonly ILogger _logger;
        
        public DeleteFaqCommandHandler(IProjectDbContext projectDbContext, ILogger logger = null)
        {
            _projectDbContext = projectDbContext;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteFaqCommand request, CancellationToken cancellationToken)
        {
            _logger?.Verbose($"Attempting to delete FAQ");
            var page = await _projectDbContext.TokenPages.Include(t => t.Faqs).FirstOrDefaultAsync(p => p.Id == request.PageId, cancellationToken);
            if (page == null)
            {
                throw new PageNotFoundException(request.PageId.ToString());
            }

            var faq = page.Faqs.FirstOrDefault(f => f.Id == request.FaqId);
            if (faq == null)
            {
                throw new FaqNotFoundException(request.FaqId.ToString());
            }

            _projectDbContext.PageFaqs.Remove(faq);
            await _projectDbContext.SaveChangesAsync(cancellationToken);
            _logger?.Verbose($"FAQ was successfully deleted");
            return Unit.Value;
        }
    }
}