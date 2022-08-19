﻿using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.ProjectManagement.Exceptions
{
    public class DocumentTypeNotFoundException : ExceptionBase
    {
        public DocumentTypeNotFoundException(string message) : base($"Document type {message} not found")
        {
        }

        public override string Code => Constants.ErrorCodes.DocumentTypeNotFound;
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}