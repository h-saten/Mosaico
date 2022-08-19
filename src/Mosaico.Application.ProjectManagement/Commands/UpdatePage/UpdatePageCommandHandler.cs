using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.DTOs.TokenPage;
using Mosaico.Authorization.Base;
using Mosaico.Base.Exceptions;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities.TokenPage;
using Mosaico.Domain.ProjectManagement.Exceptions;

namespace Mosaico.Application.ProjectManagement.Commands.UpdatePage
{
    public class UpdatePageCommandHandler : IRequestHandler<UpdatePageCommand>
    {
        private readonly IProjectDbContext _projectDbContext;
        private readonly ICurrentUserContext _currentUser;
        public UpdatePageCommandHandler(IProjectDbContext projectDbContext, ICurrentUserContext currentUser)
        {
            _projectDbContext = projectDbContext;
            _currentUser = currentUser;
        }

        public async Task<Unit> Handle(UpdatePageCommand request, CancellationToken cancellationToken)
        {
            var page = await _projectDbContext.TokenPages
                .Include(p => p.ShortDescription).ThenInclude(s => s.Translations)
                .FirstOrDefaultAsync(p => p.Id == request.PageId, cancellationToken);
            if (page == null)
            {
                throw new PageNotFoundException(request.PageId.ToString());
            }

            UpdateShortDescription(request, page);
            UpdatePageCover(request, page);
            UpdateSocialMediaLinks(request, page);
            
            await _projectDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }

        private void UpdatePageCover(UpdatePageCommand request, Page page)
        {
            var pageCover = page.PageCovers.FirstOrDefault();
            if (pageCover == null)
            {
                pageCover = new PageCover();
                page.PageCovers.Add(pageCover);
            }
            pageCover.PrimaryColor = request.Page.PrimaryColor;
            pageCover.SecondaryColor = request.Page.SecondaryColor;
            pageCover.CoverColor = request.Page.CoverColor;
        }

        private void UpdateSocialMediaLinks(UpdatePageCommand request, Page page)
        {
            foreach (var lang in request.Page.SocialMediaLinks)
            {
                var socialMediaLinks = lang.Value;
                if (socialMediaLinks.Count > Constants.SocialMediaLinkLimit)
                {
                    throw new LimitExceededException(nameof(SocialMediaLink));
                }

                if (!socialMediaLinks.Any())
                {
                    foreach (var socialMediaLink in page.SocialMediaLinks)
                    {
                        socialMediaLink.UpdateTranslation(null, lang.Key);
                    }
                }
                else
                {
                    foreach (var socialMediaLink in socialMediaLinks)
                    {
                        var socialMediaLinkInDb =
                            page.SocialMediaLinks.FirstOrDefault(sml => sml.Key == socialMediaLink.Key);
                        if (socialMediaLinkInDb == null)
                        {
                            socialMediaLinkInDb = new SocialMediaLink
                            {
                                Key = socialMediaLink.Key,
                                Title = socialMediaLink.Key,
                                Order = socialMediaLink.Order,
                                IsHidden = socialMediaLink.IsHidden,
                                Page = page,
                                PageId = page.Id
                            };
                            page.SocialMediaLinks.Add(socialMediaLinkInDb);
                        }

                        socialMediaLinkInDb.UpdateTranslation(socialMediaLink.Value, lang.Key);
                    }
                }
            }
        }

        private void UpdateShortDescription(UpdatePageCommand request, Page page)
        {
            if (request.Page?.ShortDescription != null)
            {
                page.ShortDescription ??= new ShortDescription();

                foreach (var newShortDesc in request.Page.ShortDescription)
                {
                    var translation = page.ShortDescription.GetTranslationInLanguage(newShortDesc.Key, true);
                    if (translation == null)
                    {
                        translation = new ShortDescriptionTranslation
                        {
                            Language = newShortDesc.Key,
                            Value = newShortDesc.Value
                        };
                        page.ShortDescription.Translations.Add(translation);
                    }
                    else
                    {
                        translation.Value = newShortDesc.Value;
                    }
                }
            }
        }
    }
}