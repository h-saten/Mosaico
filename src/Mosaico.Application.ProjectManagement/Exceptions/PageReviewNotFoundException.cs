using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.ProjectManagement.Exceptions
{
    public class PageReviewNotFoundException : ExceptionBase
    {
        public PageReviewNotFoundException(Guid id) : base($"Page Review with ID {id} not found")
        {
        }

        public override string Code => "PAGE_REVIEW_NOT_FOUND";
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}