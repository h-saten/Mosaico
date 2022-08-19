using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.ProjectManagement.Exceptions
{
    public class VestingNotFoundException : ExceptionBase
    {
        public VestingNotFoundException(Guid id) : base($"Vesting with id {id} was not found")
        {
        }
        
        public VestingNotFoundException(string id) : base($"Vesting with id {id} was not found")
        {
        }

        public override string Code => Constants.ErrorCodes.VestingNotFound;
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}