using MediatR;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.Entities;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Security;
using Serilog;
using System.Threading;
using System.Threading.Tasks;

namespace Mosaico.Application.Identity.Commands.SubscribeToNewsletter
{
    internal class SubscribeToNewsletterHandler : IRequestHandler<SubscribeToNewsletterCommand>
    {
        private readonly ILogger _logger;
        private readonly IIdentityContext _identityContext;

        public SubscribeToNewsletterHandler(ILogger logger, IIdentityContext identityContext)
        {
            _logger = logger;
            _identityContext = identityContext;
        }

        public async Task<Unit> Handle(SubscribeToNewsletterCommand request, CancellationToken cancellationToken)
        {
            _logger?.Verbose($"Attempting to subscribe to the newsletter");
            string userEmail = request.Email;

            if (string.IsNullOrWhiteSpace(userEmail))
            {
                throw new InvalidParameterException("email");
            }
            bool userSubscribe = await _identityContext.NewsletterSubscribers.AnyAsync(x => x.Email == userEmail);
            if(!userSubscribe)
            {
                userEmail = userEmail.ToLowerInvariant();
                NewsletterSubscribers userSubscriber = new NewsletterSubscribers
                {
                    Email = userEmail,
                    IsAccepted = true
                };
                await _identityContext.NewsletterSubscribers.AddAsync(userSubscriber);
                await _identityContext.SaveChangesAsync(cancellationToken);
            }
            return Unit.Value;
        }
    }
}
