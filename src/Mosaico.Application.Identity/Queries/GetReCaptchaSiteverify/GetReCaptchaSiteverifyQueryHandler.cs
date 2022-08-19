using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Mosaico.Base.Abstractions;

namespace Mosaico.Application.Identity.Queries.GetReCaptchaSiteVerify
{
    public class GetReCaptchaSiteVerifyQueryHandler : IRequestHandler<GetReCaptchaSiteverifyQuery,
            RecaptchaResponse>
    {
        private readonly ICaptchaVerificationClient _verificationClient;

        public GetReCaptchaSiteVerifyQueryHandler(ICaptchaVerificationClient verificationClient)
        {
            _verificationClient = verificationClient;
        }

        public async Task<RecaptchaResponse> Handle(GetReCaptchaSiteverifyQuery request,
            CancellationToken cancellationToken)
        {
            var captchaResponse = await _verificationClient.VerifyAsync(request.Response, cancellationToken);
            return captchaResponse;
        }
    }
}