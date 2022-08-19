using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.ProjectManagement.Exceptions
{
    public class EmptyContractAddressException : ExceptionBase
    {
        public EmptyContractAddressException(string projectId) : base($"Project {projectId} has no contract assigned")
        {
        }

        public override string Code => Constants.ErrorCodes.ContractAddressIsEmpty;
        public override int StatusCode => StatusCodes.Status500InternalServerError;
    }
}