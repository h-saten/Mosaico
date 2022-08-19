using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Exceptions;
using Mosaico.Authorization.Base;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities.TokenPage;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Serilog;

namespace Mosaico.Application.ProjectManagement.Commands.TokenPage.UpsertAbout
{
    public class UpsertAboutCommandHandler : IRequestHandler<UpsertAboutCommand, Guid>
    {
        private readonly IProjectDbContext _projectDbContext;
        private readonly ILogger _logger;

        public UpsertAboutCommandHandler(IProjectDbContext projectDbContext, ILogger logger = null)
        {
            _projectDbContext = projectDbContext;
            _logger = logger;
        }

        public async Task<Guid> Handle(UpsertAboutCommand request, CancellationToken cancellationToken)
        {

            if (string.IsNullOrWhiteSpace(request.Language))
            {
                request.Language = Base.Constants.Languages.English;
            }
             
            _logger?.Verbose($"Attempting to perform update on About Page");
            var page = await _projectDbContext.TokenPages.Include(x=>x.About).FirstOrDefaultAsync(f => f.Id == request.PageId,  cancellationToken);
            if (page == null)
            {
                throw new PageNotFoundException(request.PageId.ToString());
            }
            var about = page.About;
            if (about == null)
            {
                about = new About
                {
                    PageId = page.Id,
                    Page = page
                };
            }
            UpdateAboutProperties(about, request);
            _projectDbContext.AboutPages.Update(about);
            await _projectDbContext.SaveChangesAsync(cancellationToken);
            _logger?.Verbose($"About page was successfully updated");
            return about.Id;
        }

        private void UpdateAboutProperties(About about, UpsertAboutCommand request)
        {
            var fakeKey = Guid.NewGuid().ToString();
            about.Content ??= new AboutContent()
            {
                Key = fakeKey,
                Title = fakeKey
            };
            about.Content.UpdateTranslation(request.Content, request.Language);
        }

    }
}