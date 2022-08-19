using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;
using System;

namespace Mosaico.Domain.BusinessManagement.Exceptions
{
    public class AdminOnlyException : ExceptionBase
    {
        public AdminOnlyException() : base($"This page is for admins only")
        {
        }

        public override string Code => Constants.ErrorCodes.AdminOnly;
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}