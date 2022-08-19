using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Identity.Abstractions;
using Mosaico.Application.Identity.DTOs;
using Mosaico.Authorization.Base.Configurations;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.Entities;
using Mosaico.Events.Base;
using Mosaico.Events.Identity;
using Serilog;

namespace Mosaico.Application.Identity.Commands.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserCommandResponse>
    {
        private readonly ILogger _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventFactory _eventFactory;
        private readonly IIdentityContext _identityContext;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IUserLoginAttemptFactory _attemptFactory;
        private readonly IEventService _idEventService;
        private readonly IDeviceAuthorizationVerifier _deviceAuthorizationVerifier;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly AuthenticationConfiguration _authenticationConfiguration;

        public LoginUserCommandHandler(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, 
            IEventPublisher eventPublisher, 
            IEventFactory eventFactory, 
            IIdentityContext identityContext,
            IIdentityServerInteractionService interaction, 
            IUserLoginAttemptFactory attemptFactory, 
            IEventService idEventService,
            IDeviceAuthorizationVerifier deviceAuthorizationVerifier, 
            IPasswordHasher<ApplicationUser> passwordHasher, 
            AuthenticationConfiguration authenticationConfiguration, 
            ILogger logger = null)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _eventPublisher = eventPublisher;
            _eventFactory = eventFactory;
            _identityContext = identityContext;
            _interaction = interaction;
            _attemptFactory = attemptFactory;
            _idEventService = idEventService;
            _deviceAuthorizationVerifier = deviceAuthorizationVerifier;
            _passwordHasher = passwordHasher;
            _authenticationConfiguration = authenticationConfiguration;
            _logger = logger;
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.ReturnUrl))
            {
                request.ReturnUrl = request?.AfterLoginRedirectUrl;
            }
            
            var context = await _interaction.GetAuthorizationContextAsync(request.ReturnUrl);
            var redirectUrl = context?.RedirectUri ?? request.ReturnUrl;

            var result = new LoginResult(
                LoginResponseTypeDTO.Error, 
                request.ReturnUrl, redirectUrl);

            var redirectUrlIsWhitelisted = _authenticationConfiguration.RedirectUris.Contains(redirectUrl) is true;
            if (redirectUrlIsWhitelisted is false)
            {
                return new LoginUserCommandResponse(result);
            }
            
            _logger?.Verbose($"Attempting to authorize the user");

            var normalizedEmail = request.Email.ToUpperInvariant();
            var user = await _identityContext.Users.FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail, cancellationToken);

            if (user != null && !string.IsNullOrWhiteSpace(user.PasswordHash))
            {
                result.IsAuthenticatorEnabled = user.TwoFactorEnabled;
                var passwordCheck = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
                if (passwordCheck == PasswordVerificationResult.Success)
                {
                    if (user.IsDeactivated)
                    {
                        _logger?.Verbose($"User {request.Email} deactivated");
                        result.Type = LoginResponseTypeDTO.Deactivated;
                        return new LoginUserCommandResponse(result);
                    }

                    if(user.MarkedForDeletion)
                    {
                        _logger?.Verbose($"User {user.Email} is marked for deletion");
                        result.Type = LoginResponseTypeDTO.InvalidData;
                        return new LoginUserCommandResponse(result);
                    }
                }
                else
                {
                    _logger?.Verbose($"User {request.Email} is not allowed. Wrong data");
                    result.Type = LoginResponseTypeDTO.InvalidData;
                    return new LoginUserCommandResponse(result);
                }
                
                var signInResult = await _signInManager.PasswordSignInAsync(request.Email, request.Password, request.Remember, true);
               
                if (signInResult.Succeeded)
                {
                    (result, signInResult) = await CheckDeviceVerificationAsync(request, cancellationToken, user, result, signInResult);
                }
                
                _logger?.Verbose($"User {request.Email} successfully logged in");
                if (signInResult.Succeeded)
                {
                    await _idEventService.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName, clientId: context?.Client.ClientId));
                    await SaveLoginAttemptAsync(user, signInResult);
                    result.Type = LoginResponseTypeDTO.Succeeded;
                    await PublishEventsAsync(user);
                }
                else
                {
                    if (signInResult.RequiresTwoFactor)
                    {
                        _logger?.Verbose($"User {request.Email} requires MFA");
                        result.Type = LoginResponseTypeDTO.RequiresTwoFactor;
                    }
                    else if (signInResult.IsLockedOut)
                    {
                        _logger?.Verbose($"User {request.Email} is locked");
                        result.Type = LoginResponseTypeDTO.LockedOut;
                    }
                    else
                    {
                        await SaveLoginAttemptAsync(user, signInResult);
                        await _idEventService.RaiseAsync(new UserLoginFailureEvent(request.Email, "invalid credentials", clientId:context?.Client.ClientId));
                        _logger?.Verbose($"User {request.Email} is not allowed");
                        result.Type = LoginResponseTypeDTO.InvalidData;
                        await LockUserIfExceedsFailedAttemptsAsync(user);
                    }
                }
            }
            else
            {
                _logger?.Verbose($"User {request.Email} is not allowed. Wrong data");
                result.Type = LoginResponseTypeDTO.InvalidData;
            }
            
            return new LoginUserCommandResponse(result);
        }

        private async Task<(LoginResult, SignInResult)> CheckDeviceVerificationAsync(LoginUserCommand request, CancellationToken cancellationToken,
            ApplicationUser user, LoginResult result, SignInResult signInResult)
        {
            var deviceAuthorizationResult = await _deviceAuthorizationVerifier.VerifyAsync(x =>
            {
                x.User = user;
                x.AgentInfo = request.AgentInfo;
                x.IP = request.IP;
                x.AuthorizationCode = request.AuthorizeDeviceCode;
            }, cancellationToken);

            if (deviceAuthorizationResult.Success is false)
            {
                await _signInManager.SignOutAsync();

                result.DeviceVerificationRequired = true;
                signInResult = SignInResult.TwoFactorRequired;
                result.DeviceAuthorization = new DeviceAuthorizationDto
                {
                    FailureReason = deviceAuthorizationResult.FailureReason,
                    CanGenerateNewCode = deviceAuthorizationResult.CanGenerateNewCode,
                    LastGeneratedCodeStillValid = deviceAuthorizationResult.LastGeneratedCodeStillValid,
                    CodeExpiryAt = deviceAuthorizationResult.CodeExpiryAt,
                    DeviceVerificationType = deviceAuthorizationResult.VerificationType != null ?
                        (DeviceVerificationTypeDTO) deviceAuthorizationResult.VerificationType : null
                };
            }
            
            return (result, signInResult);
        }

        private async Task LockUserIfExceedsFailedAttemptsAsync(ApplicationUser user)
        {
            var treshold = DateTimeOffset.UtcNow.AddMinutes(-Constants.LockoutParameters.LoginFailuresThresholdInMinutes);
            var loginAttempts = await _identityContext.LoginAttempts.Where(a => a.UserId == user.Id && !a.Successful && a.LoggedInAt > treshold).CountAsync();
            if (loginAttempts >= Constants.LockoutParameters.FailAttemptsCountBeforeLockout)
            {
                _logger?.Information($"User was locked due to exceeded amount of failed logins");
                await _userManager.SetLockoutEnabledAsync(user, true);
                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddMinutes(Constants.LockoutParameters.LockoutPeriodInMinutes));
            }
        }

        private async Task PublishEventsAsync(ApplicationUser user)
        {
            try
            {
                await PublishEventAsync(user.Id);
            }
            catch (Exception ex)
            {
                _logger?.Error($"Error during submitting login event {ex.Message}/{ex.StackTrace}");
            }
        }

        private async Task PublishEventAsync(string userId)
        {
            var e = _eventFactory.CreateEvent(Events.Identity.Constants.EventPaths.Users,
                new UserLoggedInEvent(userId));
            await _eventPublisher.PublishAsync(Events.Identity.Constants.EventPaths.Users, e);
        }

        private async Task SaveLoginAttemptAsync(ApplicationUser user, SignInResult result)
        {
            var attempt = await _attemptFactory.CreateUserLoginAttempt(user, result);
            if (attempt != null)
            {
                _identityContext.LoginAttempts.Add(attempt);
                if (result.Succeeded)
                {
                    user.UpdateLastLogin();
                }
                await _identityContext.SaveChangesAsync();
                _logger?.Verbose($"Login attempt was successfully saved");
            }
        }
    }
}