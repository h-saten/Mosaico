using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.ProjectManagement.Commands.SubmitProjectForReview
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    public class SubmitProjectForReviewCommand : IRequest
    {
        public Guid ProjectId { get; set; }
    }
}