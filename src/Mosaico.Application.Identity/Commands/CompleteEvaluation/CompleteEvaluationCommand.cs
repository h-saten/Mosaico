using System.Collections.Generic;
using System.Text.Json.Serialization;
using MediatR;
using Mosaico.Application.Identity.DTOs;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Identity.Commands.CompleteEvaluation
{
    [Restricted(nameof(UserId), Authorization.Base.Constants.DefaultRoles.Self)]
    public class CompleteEvaluationCommand : IRequest
    {
        [JsonIgnore]
        public string UserId { get; set; }
        public List<UserEvaluationQuestionDTO> Responses { get; set; } = new List<UserEvaluationQuestionDTO>();
    }
}