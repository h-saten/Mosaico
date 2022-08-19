using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Mosaico.Base.Abstractions;
using Mosaico.Base.Configurations;
using Mosaico.Base.Exceptions;
using Newtonsoft.Json;
using Serilog;

namespace Mosaico.Base
{
    public class RecaptchaVerificationClient : ICaptchaVerificationClient
    {
        private readonly RecaptchaConfiguration _recaptchaConfiguration;
        private readonly ILogger _logger;

        public RecaptchaVerificationClient(RecaptchaConfiguration recaptchaConfiguration, ILogger logger = null)
        {
            _recaptchaConfiguration = recaptchaConfiguration;
            _logger = logger;
        }

        public async Task<RecaptchaResponse> VerifyAsync(string code, CancellationToken t = new())
        {

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetStringAsync(
                    $"{_recaptchaConfiguration.Url}?secret={_recaptchaConfiguration.ApiSecret}&response={code}",
                    t);

                var reCaptcharesponse = JsonConvert.DeserializeObject<RecaptchaResponse>(response);
                if (reCaptcharesponse == null)
                {
                    throw new RecaptchaException("Invalid response from endpoint");
                }

                if (reCaptcharesponse.ErrorCodes != null && reCaptcharesponse.ErrorCodes.Any())
                {
                    var errors = string.Join(',', reCaptcharesponse?.ErrorCodes);
                    _logger?.Warning($"Captcha validation failed: {errors}");
                }

                return reCaptcharesponse;
            }
        }
    }
}