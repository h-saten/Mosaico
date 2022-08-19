using MediatR;
using Mosaico.Base.Abstractions;

namespace Mosaico.Application.Identity.Queries.GetReCaptchaSiteVerify
{
    public class GetReCaptchaSiteverifyQuery : IRequest<RecaptchaResponse>
    {
        public string Response { get; set; }
    }
}