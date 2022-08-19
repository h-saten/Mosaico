using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;
using System;

namespace Mosaico.Domain.BusinessManagement.Exceptions
{
    public class TeamMemberNotFoundException : ExceptionBase
    {
        public TeamMemberNotFoundException(string name) : base($"Team member {name} was not found")
        {
        }

        public TeamMemberNotFoundException(Guid id) : base($"Team member with id {id} was not found")
        {
        }

        public override string Code => Constants.ErrorCodes.TeamMemberNotFound;
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}