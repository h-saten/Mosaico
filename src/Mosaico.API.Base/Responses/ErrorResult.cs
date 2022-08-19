using Microsoft.AspNetCore.Mvc;
using Mosaico.Base.Exceptions;
using Newtonsoft.Json;

namespace Mosaico.API.Base.Responses
{
    public class ErrorResult : JsonResult
    {
        public ErrorResult(ExceptionBase exception) : base(new
            {code = exception.Code, message = exception.Message, extraData = exception.ExtraData, ok = false})
        {
            Ok = false;
            Code = exception.Code;
            Message = exception.Message;
            ExtraData = exception.ExtraData;
        }

        public ErrorResult(ExceptionBase exception, JsonSerializerSettings serializerSettings) : base(
            new {code = exception.Code, message = exception.Message, extraData = exception.ExtraData, ok = false},
            serializerSettings)
        {
            Ok = false;
            Code = exception.Code;
            Message = exception.Message;
            ExtraData = exception.ExtraData;
        }

        public ErrorResult(string errorCode, string message, object extraData = null) : base(new
            {code = errorCode, message, extraData, ok = false})
        {
            Ok = false;
            Code = errorCode;
            Message = message;
            ExtraData = extraData;
        }
        
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("extraData")]
        public object ExtraData { get; set; }
        [JsonProperty("ok")]
        public bool Ok { get; set; }
        
        [System.Text.Json.Serialization.JsonIgnore] 
        public new int? StatusCode { get; set; }

        [System.Text.Json.Serialization.JsonIgnore] 
        public new string ContentType { get; set; }
    }
}