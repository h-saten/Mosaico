using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.ProjectManagement.Exceptions.Airdrop
{
    public class AirdropCampaignAlreadyExists : ExceptionBase
    {
        public AirdropCampaignAlreadyExists(string message) : base($"Airdrop with name {message} already exists")
        {
        }

        public override string Code => "AIRDROP_ALREADY_EXISTS";
        public override int StatusCode => StatusCodes.Status409Conflict;
    }
}