using System;
using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Queries.GetProjectMembers
{
    public class GetProjectMembersQueryValidator : AbstractValidator<GetProjectMembersQuery>
    {
        public GetProjectMembersQueryValidator()
        {
            RuleFor(p => p.ProjectId).NotEmpty().Must(c => c != Guid.Empty);
        }
    }
}