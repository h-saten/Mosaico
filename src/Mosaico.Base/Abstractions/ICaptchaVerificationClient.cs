using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Mosaico.Base.Abstractions
{
    public class RecaptchaResponse
    {
        public bool Success { get; set; }
        public DateTime Challenge_ts { get; set; }
        
        [JsonProperty("error-codes")]
        public IEnumerable<string> ErrorCodes { get; set; }
        public long Score { get; set; }
    }
    
    public interface ICaptchaVerificationClient
    {
        Task<RecaptchaResponse> VerifyAsync(string code, CancellationToken t = new());
    }
}