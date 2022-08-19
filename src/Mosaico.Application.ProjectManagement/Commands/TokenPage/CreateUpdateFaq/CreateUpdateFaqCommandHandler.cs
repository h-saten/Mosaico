using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Exceptions;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities.TokenPage;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Serilog;

namespace Mosaico.Application.ProjectManagement.Commands.TokenPage.CreateUpdateFaq
{
    public class CreateUpdateFaqCommandHandler : IRequestHandler<CreateUpdateFaqCommand, Guid>
    {
        private readonly IProjectDbContext _projectDbContext;
        private readonly ILogger _logger;

        public CreateUpdateFaqCommandHandler(IProjectDbContext projectDbContext, ILogger logger = null)
        {
            _projectDbContext = projectDbContext;
            _logger = logger;
        }

        public async Task<Guid> Handle(CreateUpdateFaqCommand request, CancellationToken cancellationToken)
        {
            var faqCount = await _projectDbContext.PageFaqs
                .CountAsync(p => p.PageId == request.PageId, cancellationToken: cancellationToken);
            if (faqCount >= Constants.FaqLimit)
            {
                throw new FaqLimitExceededException(request.PageId.ToString());
            }

            if (string.IsNullOrWhiteSpace(request.Language))
            {
                request.Language = Base.Constants.Languages.English;
            }

            if (request.Id.HasValue)
            {
                _logger?.Verbose($"Id was supplied to update FAQ. Attempting to perform upgrade");
                var faq = await _projectDbContext.PageFaqs.FirstOrDefaultAsync(f => f.Id == request.Id,
                    cancellationToken);
                if (faq == null)
                {
                    throw new FaqNotFoundException(request.Id.ToString());
                }

                UpdateFaqProperties(faq, request);
                _projectDbContext.PageFaqs.Update(faq);
                await _projectDbContext.SaveChangesAsync(cancellationToken);
                _logger?.Verbose($"FAQ was successfully updated");
                return faq.Id;
            }
            else
            {
                _logger?.Verbose($"ID was not supplied. FAQ is new. Creating");
                var page = await _projectDbContext.TokenPages.FirstOrDefaultAsync(p => p.Id == request.PageId,
                    cancellationToken);
                if (page == null)
                {
                    throw new PageNotFoundException(request.PageId.ToString());
                }

                var faq = new Faq
                {
                    PageId = page.Id,
                    Page = page
                };
                UpdateFaqProperties(faq, request);
                _projectDbContext.PageFaqs.Add(faq);
                await _projectDbContext.SaveChangesAsync(cancellationToken);
                _logger?.Verbose($"FAQ was successfully created");
                return faq.Id;
            }
        }

        private void UpdateFaqProperties(Faq faq, CreateUpdateFaqCommand request)
        {
            var fakeKey = Guid.NewGuid().ToString();
            faq.Content ??= new FaqContent()
            {
                Key = fakeKey,
                Title = fakeKey
            };
            faq.Content.UpdateTranslation(request.Content, request.Language);
            faq.Title ??= new FaqTitle
            {
                Key = fakeKey,
                Title = fakeKey
            };
            faq.Title.UpdateTranslation(request.Title, request.Language);
            faq.Order = request.Order;
            faq.IsHidden = request.IsHidden;
        }
    }
}