using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.DocumentExport.CKEditor.Exceptions
{
    public class CKEditorExportException : ExceptionBase
    {
        public CKEditorExportException(string message) : base(message)
        {
        }

        public override string Code => "EXPORT_EXCEPTION";
        public override int StatusCode => StatusCodes.Status500InternalServerError;
    }
}