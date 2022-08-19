using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;
using System;

namespace Mosaico.Domain.BusinessManagement.Exceptions
{
    public class ShareholdersNotFoundException : ExceptionBase
    {
        public ShareholdersNotFoundException() : base($"Shareholders were not found")
        {
        }


        public override string Code => Constants.ErrorCodes.ShareholdersNotFound;
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}