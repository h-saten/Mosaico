using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Mosaico.API.Base.Exceptions;

namespace Mosaico.Identity.Infrastructure
{
    public class ApiResponse
    {
        public string Message { get; set; }
        public string Detail { get; set; }
        public string ReferenceId { get; set; }
        public ValidationErrorCollection Errors { get; set; }

        public ApiResponse(string message)
        {
            this.Message = message;
        }
        
        public ApiResponse(string message, string detail, string referenceId)
        {
            Message = message;
            Detail = detail;
            ReferenceId = referenceId;
        }

        public ApiResponse(ModelStateDictionary modelState)
        {
            if (modelState != null && modelState.Any(m => m.Value.Errors.Count > 0))
            {
                Message = "Please correct the specified errors and try again.";
                //errors = modelState.SelectMany(m => m.Value.Errors).ToDictionary(m => m.Key, m=> m.ErrorMessage);
                //errors = modelState.SelectMany(m => m.Value.Errors.Select( me => new KeyValuePair<string,string>( m.Key,me.ErrorMessage) ));
                //errors = modelState.SelectMany(m => m.Value.Errors.Select(me => new ModelError { FieldName = m.Key, ErrorMessage = me.ErrorMessage }));
            }
        }
    }
}