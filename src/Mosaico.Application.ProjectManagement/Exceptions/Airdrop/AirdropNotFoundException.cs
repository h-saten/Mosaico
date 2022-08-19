using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.ProjectManagement.Exceptions.Airdrop
{
    public class AirdropNotFoundException : ExceptionBase
    {
        public AirdropNotFoundException(string message) : base($"Airdrop {message} not found")
        {
        }

        public override string Code => "AIRDROP_NOT_FOUND";
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}