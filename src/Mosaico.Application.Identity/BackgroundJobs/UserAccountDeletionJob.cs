using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Mosaico.Application.Identity.Exceptions;
using Mosaico.BackgroundJobs.Base;
using Mosaico.BackgroundJobs.Hangfire.Abstractions;
using Mosaico.BackgroundJobs.Hangfire.Configurations;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.Entities;
using Mosaico.Domain.Identity.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.Identity;
using Mosaico.SDK.BusinessManagement.Abstractions;
using Mosaico.SDK.ProjectManagement.Abstractions;
using Serilog;
using Constants = Mosaico.Application.Identity.Constants;

namespace Mosaico.Application.Identity.BackgroundJobs
{
    [BackgroundJob(Constants.Jobs.UserAccountDeletionJob, IsRecurring = true)]
    public class UserAccountDeletionJob : HangfireBackgroundJobBase
    {
        private readonly ILogger _logger;
        private readonly IIdentityContext _identityContext;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventFactory _eventFactory;
        private readonly HangfireConfig _hangfireConfiguration;


        public UserAccountDeletionJob(HangfireConfig hangfireConfiguration, IIdentityContext identityContext, IEventPublisher eventPublisher, IEventFactory eventFactory, ILogger logger = null)
        {
            _identityContext = identityContext;
            _eventPublisher = eventPublisher;
            _eventFactory = eventFactory;
            _logger = logger;
            _hangfireConfiguration = hangfireConfiguration;

        }
        public override async Task ExecuteAsync(object parameters = null)
        {
            var now = DateTimeOffset.UtcNow;
            var accountDeletionFrequency = _hangfireConfiguration.AccountDeletionFrequency;
            try
            {
                _logger?.Verbose($"Reccuring Job User Account Deletion started at {now:g}");

                using (var transaction = _identityContext.BeginTransaction())
                {
                    try
                    {
                        var requests = await _identityContext.DeletionRequests.ToListAsync();
                        foreach (var req in requests)
                        {
                            if (Math.Abs((req.DeletionRequestedAt - DateTimeOffset.UtcNow).TotalHours) >= accountDeletionFrequency)
                            {
                                var userToPermissionEntity = await _identityContext.UserToPermission.Where(x => x.UserId == req.UserId).ToListAsync();
                                foreach (var userPermission in userToPermissionEntity)
                                {
                                    _identityContext.UserToPermission.Remove(userPermission);
                                }
                                _identityContext.DeletionRequests.Remove(req);
                                var loginAttemptsEntity = await _identityContext.LoginAttempts.Where(x => x.UserId == req.UserId).ToListAsync();
                                foreach (var loginAttempt in loginAttemptsEntity)
                                {
                                    _identityContext.LoginAttempts.Remove(loginAttempt);
                                }
                                var userEntity = await _identityContext.Users.FirstOrDefaultAsync(x => x.Id == req.UserId);
                                if (userEntity != null)
                                    _identityContext.Users.Remove(userEntity);
                                await SendEventsAsync(req.UserId);
                                await _identityContext.SaveChangesAsync();
                            }
                        }

                        await transaction.CommitAsync();
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.Error(ex.Message + ex.StackTrace);
                throw;
            }
        }

        private async Task SendEventsAsync(string userId)
        {
            try
            {
                var @event = _eventFactory.CreateEvent(Events.Identity.Constants.EventPaths.Users, new UserDeletedEvent(userId));
                await _eventPublisher.PublishAsync(Events.Identity.Constants.EventPaths.Users, @event);
            }
            catch (Exception ex)
            {
                _logger?.Error($"{ex.Message} + {ex.StackTrace}");
            }
        }
    }
}