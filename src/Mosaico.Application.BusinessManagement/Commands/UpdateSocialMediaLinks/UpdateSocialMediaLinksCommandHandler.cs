using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.BusinessManagement.Abstractions;
using Mosaico.Domain.BusinessManagement.Entities;
using Mosaico.Events.Base;
using Mosaico.Events.BusinessManagement;
using Serilog;

namespace Mosaico.Application.BusinessManagement.Commands.UpdateSocialMediaLinks
{
    public class UpdateSocialMediaLinksCommandHandler : IRequestHandler<UpdateSocialMediaLinksCommand>
    {
        private readonly IBusinessDbContext _context;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public UpdateSocialMediaLinksCommandHandler(IBusinessDbContext context, IEventFactory eventFactory, IEventPublisher eventPublisher, IMapper mapper, ILogger logger = null)
        {
            _context = context;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateSocialMediaLinksCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger?.Verbose($"Attempting to update a company social links");
                var socialMedia = await _context.SocialMediaLinks
                    .FirstOrDefaultAsync(s=>s.CompanyId == request.CompanyId, cancellationToken);
                _logger?.Verbose($"Company was found");

                if (socialMedia == null)
                {
                    socialMedia = new SocialMediaLinks
                    {
                        CompanyId = request.CompanyId,
                        Telegram = request.Telegram,
                        Youtube = request.Youtube,
                        LinkedIn = request.LinkedIn,
                        Facebook = request.Facebook,
                        Twitter = request.Twitter,
                        Instagram = request.Instagram,
                        Medium = request.Medium
                    };
                    _context.SocialMediaLinks.Add(socialMedia);
                }
                else
                {
                    socialMedia.Telegram = request.Telegram;
                    socialMedia.Youtube = request.Youtube;
                    socialMedia.LinkedIn = request.LinkedIn;
                    socialMedia.Facebook = request.Facebook;
                    socialMedia.Twitter = request.Twitter;
                    socialMedia.Instagram = request.Instagram;
                    socialMedia.Medium = request.Medium;
                    _context.SocialMediaLinks.Update(socialMedia);
                }
                
                
                await _context.SaveChangesAsync(cancellationToken);
                _logger?.Verbose($"Social media for that company was successfully updated");
                _logger?.Verbose($"Attempting to send events");
                await PublishCompanySocialMediaUpdatedEventAsync(socialMedia);
                _logger?.Verbose($"Events were sent");
                return Unit.Value;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task PublishCompanySocialMediaUpdatedEventAsync(SocialMediaLinks social)
        {
            var eventPayload = new CompanySocialMediaUpdatedEvent(social.Id);
            var @event = _eventFactory.CreateEvent(Events.BusinessManagement.Constants.EventPaths.Companies, eventPayload);
            await _eventPublisher.PublishAsync(@event.Source, @event);
        }
    }
}