using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Identity.Abstractions;
using Mosaico.Application.Identity.DTOs;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.Entities;
using Mosaico.Domain.Identity.ValueObjects;
using Mosaico.Integration.Sms.Abstraction;
using Serilog;

namespace Mosaico.Application.Identity.Services
{
    public class DeviceAuthorizationVerifier : IDeviceAuthorizationVerifier
    {
        private static string InvalidCodeErrorCode = "INVALID_CODE";
        private static string ExceededMaxAttemptsAmount = "EXCEEDED_MAX_ATTEMPTS_AMOUNT";
        private static string GeneratedCodeIsStillValid = "VALID_ALREADY_GENERATED_CODE";
        
        private readonly ILogger _logger;
        private readonly IIdentityContext _identityContext;
        private readonly ISecurityCodeRepository _securityCodeRepository;
        private readonly IIdentityEmailService _identityEmailService;
        private readonly ISmsSender _smsSender;
        private readonly IPhoneNumberConfirmationCodesRepository _numberConfirmationCodesRepository;

        public DeviceAuthorizationVerifier(
            IIdentityContext identityContext, 
            ISecurityCodeRepository securityCodeRepository, 
            IIdentityEmailService identityEmailService, 
            ISmsSender smsSender, 
            IPhoneNumberConfirmationCodesRepository numberConfirmationCodesRepository,
            ILogger logger = null)
        {
            _identityContext = identityContext;
            _securityCodeRepository = securityCodeRepository;
            _identityEmailService = identityEmailService;
            _smsSender = smsSender;
            _numberConfirmationCodesRepository = numberConfirmationCodesRepository;
            _logger = logger;
        }

        private static DeviceAuthorizationResult SuccessResult => new() 
        {
            Success = true,
            VerificationType = null
        };

        private static DeviceAuthorizationResult VerificationFailedResult(string failureReason, DateTimeOffset? codeExpiryAt = null) => new() 
        {
            Success = false,
            VerificationType = null,
            FailureReason = failureReason,
            CanGenerateNewCode = failureReason != ExceededMaxAttemptsAmount,
            LastGeneratedCodeStillValid = failureReason == GeneratedCodeIsStillValid,
            CodeExpiryAt = codeExpiryAt
        };

        private static DeviceAuthorizationResult VerificationRequiredResult(DeviceVerificationTypeDTO verificationType, DateTimeOffset codeExpiryAt, string failureReason) => new() 
        {
            Success = false,
            VerificationType = verificationType,
            CodeExpiryAt = codeExpiryAt,
            FailureReason = failureReason,
            CanGenerateNewCode = failureReason != ExceededMaxAttemptsAmount
        };

        public async Task<DeviceAuthorizationResult> VerifyAsync(Action<DeviceVerificationParams> verificationParams, CancellationToken cancellationToken)
        {
            var parameters = new DeviceVerificationParams();
            verificationParams.Invoke(parameters);

            var expiryTime = DateTimeOffset.UtcNow.AddHours(-5);
            
            var authorizedDeviceExist = await _identityContext
                .AuthorizedDevices
                .AsNoTracking()
                .Where(x => x.UserId == parameters.User.Id && x.IP == parameters.IP && x.AgentInfo == parameters.AgentInfo && expiryTime >= x.CreatedAt)
                .AnyAsync(cancellationToken);

            if (authorizedDeviceExist)
            {
                return SuccessResult;
            }

            var verificationType = parameters.User.PhoneNumberConfirmed is true
                ? DeviceVerificationTypeDTO.Sms
                : DeviceVerificationTypeDTO.Email;
            
            var verifyPassedCode = string.IsNullOrEmpty(parameters.AuthorizationCode) is false;
            if (verifyPassedCode)
            {
                return await TryVerifyDeviceAuthorizationCodeAsync(verificationType, parameters, cancellationToken);
            }

            return await GenerateAndSendNewVerificationCodeAsync(verificationType, parameters, cancellationToken);
        }

        private async Task<DeviceAuthorizationResult> TryVerifyDeviceAuthorizationCodeAsync(
            DeviceVerificationTypeDTO verificationType, DeviceVerificationParams parameters, CancellationToken cancellationToken)
        {
            DeviceAuthorizationResult result; 
            if (verificationType == DeviceVerificationTypeDTO.Sms)
            {
                result = await SmsVerificationAsync(parameters);
            }
            else
            {
                result = await EmailVerificationAsync(parameters, cancellationToken);
            }

            if (result.Success)
            {
                await AddAuthorizedDevice(parameters, cancellationToken);
            }

            await _identityContext.SaveChangesAsync(cancellationToken);

            return result;
        }

        private async Task<DeviceAuthorizationResult> SmsVerificationAsync(DeviceVerificationParams parameters)
        {
            var code = await _numberConfirmationCodesRepository
                .GetLastlyGeneratedCodeAsync(parameters.User.Id);

            var isValidCode = code != null && code.Code == parameters.AuthorizationCode && !code.Used;

            if (isValidCode is false)
            {
                return VerificationFailedResult(InvalidCodeErrorCode, code?.ExpiresAt);
            }

            await _numberConfirmationCodesRepository
                .SetSecurityCodeUsed(code.Id);

            return SuccessResult;
        }

        private async Task<DeviceAuthorizationResult> EmailVerificationAsync(DeviceVerificationParams parameters, CancellationToken cancellationToken)
        {
            var code = await _securityCodeRepository
                .GetCodeAsync(parameters.User.Id, SecurityCode.DeviceAuthorizationContext);

            var isValidCode = code != null && code.Code == parameters.AuthorizationCode && !code.IsUsed;
            
            if (!isValidCode)
            {
                return VerificationFailedResult(InvalidCodeErrorCode, code?.ExpiresAt);
            }

            await _securityCodeRepository.SetSecurityCodeUsed(code.Id);
            
            return SuccessResult;
        }
        
        private async Task AddAuthorizedDevice(DeviceVerificationParams parameters, CancellationToken cancellationToken)
        {
            var authorizedDevice = new AuthorizedDevice
            {
                UserId = parameters.User.Id,
                IP = parameters.IP,
                AgentInfo = parameters.AgentInfo
            };
            await _identityContext.AuthorizedDevices.AddAsync(authorizedDevice, cancellationToken);
        }

        private async Task<DeviceAuthorizationResult> GenerateAndSendNewVerificationCodeAsync(
            DeviceVerificationTypeDTO verificationType, DeviceVerificationParams parameters, CancellationToken cancellationToken)
        {
            DateTimeOffset codeExpiryAt = default;
            
            var (lastGeneratedCodeIsStillValid, codeValidTo) = await LastGeneratedCodeIsStillValidGuard(parameters.User, verificationType);

            if (lastGeneratedCodeIsStillValid is true)
            {
                return VerificationFailedResult(GeneratedCodeIsStillValid, codeValidTo);
            }

            var canGenerateNewCode = await MaxVerificationsAmountGuard(parameters.User, verificationType);

            if (canGenerateNewCode is false)
            {
                return VerificationFailedResult(ExceededMaxAttemptsAmount);
            }
            
            if (verificationType == DeviceVerificationTypeDTO.Sms)
            {
                try
                {
                    var code = await _numberConfirmationCodesRepository
                        .CreateCodeAsync(parameters.User.Id, new PhoneNumber(parameters.User.PhoneNumber));
                    codeExpiryAt = code.ExpiresAt;
                    await _smsSender.SendAsync(new SmsMessage
                    {
                        Content = $"Your code: {code.Code}",
                        Subject = "Device authorization code",
                        RecipientsPhoneNumber = code.PhoneNumber.Value
                    }, cancellationToken);
                }
                catch (Exception)
                {
                    _logger.Error("Cannot send device authorization code via sms.");
                    verificationType = DeviceVerificationTypeDTO.Email;
                }
            }
            
            if (verificationType == DeviceVerificationTypeDTO.Email)
            {
                var code = await _securityCodeRepository.CreateCodeAsync(SecurityCode.RandomCode(8), parameters.User.Id,
                    SecurityCode.DeviceAuthorizationContext);
                codeExpiryAt = code.ExpiresAt;
                await _identityEmailService.SendDeviceAuthorizationCode(parameters.User.Email, code.Code);
            }

            return VerificationRequiredResult(verificationType, codeExpiryAt, null);
        }

        private async Task<(bool, DateTimeOffset? validTo)> LastGeneratedCodeIsStillValidGuard(ApplicationUser user, DeviceVerificationTypeDTO verificationFlowType)
        {
            if (verificationFlowType == DeviceVerificationTypeDTO.Sms)
            {
                var lastSmsCode = await _numberConfirmationCodesRepository
                    .GetLastlyGeneratedCodeAsync(user.Id);
                
                return (lastSmsCode != null, lastSmsCode?.ExpiresAt);
            }

            var lastEmailCode =
                await _securityCodeRepository.GetCodeAsync(user.Id, SecurityCode.DeviceAuthorizationContext);

            return (lastEmailCode != null, lastEmailCode?.ExpiresAt);
        }

        private async Task<bool> MaxVerificationsAmountGuard(ApplicationUser user, DeviceVerificationTypeDTO verificationFlowType)
        {
            int maxAttemptsAmount = 3;
            var attemptsPeriod = DateTimeOffset.UtcNow.AddMinutes(-10);

            int attemptsAmount;
            if (verificationFlowType == DeviceVerificationTypeDTO.Sms)
            {
                attemptsAmount = await _identityContext
                    .PhoneNumberConfirmationCodes
                    .Where(x => x.UserId == user.Id && x.CreatedAt >= attemptsPeriod)
                    .CountAsync();
            }
            else
            {
                attemptsAmount = await _identityContext
                    .SecurityCodes
                    .Where(x => x.UserId == user.Id && x.CreatedAt >= attemptsPeriod)
                    .CountAsync();
            }

            return attemptsAmount < maxAttemptsAmount;
        }
    }
}