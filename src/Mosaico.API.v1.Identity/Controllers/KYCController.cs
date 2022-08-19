using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mosaico.API.Base.Filters;
using Mosaico.API.Base.Responses;
using Mosaico.Application.Identity.Abstractions;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.Entities;
using Mosaico.KYC.Passbase.Abstractions;
using Mosaico.KYC.Passbase.Models;
using Serilog;

namespace Mosaico.API.v1.Identity.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/kyc")]
    [Route("api/v{version:apiVersion}/kyc")]
    [Produces("application/json")]
    [AllowAnonymous]
    [APIExceptionFilter]
    public class KYCController : ControllerBase
    {
        private readonly IPassbaseClient _passbaseClient;
        private readonly IIdentityEmailService _emailService;
        private readonly IKycService _kycService;
        private readonly IIdentityContext _identityContext;
        private readonly ILogger _logger;

        public KYCController(IPassbaseClient passbaseClient, IIdentityContext identityContext, IKycService kycService, IIdentityEmailService emailService, ILogger logger)
        {
            _passbaseClient = passbaseClient;
            _identityContext = identityContext;
            _kycService = kycService;
            _emailService = emailService;
            _logger = logger;
        }

        [HttpPost("passbase")]
        public async Task<IActionResult> PassbaseStatusUpdated([FromBody] PassbaseVerificationPayload payload)
        {
            _logger.Information($"Received KYC verification request from passbase: {payload?.Event} - {payload?.Status} - {payload?.Key}");
            if (!string.IsNullOrWhiteSpace(payload?.Key) && (payload.Event == PassbaseVerificationEventType.VERIFICATION_REVIEWED || payload.Event == PassbaseVerificationEventType.VERIFICATION_COMPLETED))
            {
                var identity = await _passbaseClient.GetIdentityAsync(payload.Key);
                var email = identity.Owner?.Email?.Trim().ToUpperInvariant();
                if (!string.IsNullOrWhiteSpace(email))
                {
                    var user = await _identityContext.Users.FirstOrDefaultAsync(e => e.NormalizedEmail == email);
                    if (user != null)
                    {
                        if (identity.Status == "approved" && user.AMLStatus != AMLStatus.Confirmed)
                        {
                            await _kycService.SetUserVerifiedByEmailAsync(identity.Owner.Email,
                                identity?.Owner?.FirstName,
                                identity?.Owner?.LastName);
                            await _emailService.SendKycCompletedSuccessfullyAsync(user.Email, user.Language);
                        }
                        else if (identity.Status == "declined")
                        {
                            await _kycService.SetUserDeclinedByEmailAsync(identity.Owner.Email);
                        }
                    }
                }
            }

            return new SuccessResult(true);
        }
    }
}