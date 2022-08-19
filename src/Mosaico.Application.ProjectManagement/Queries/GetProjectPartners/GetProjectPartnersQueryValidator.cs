using System;
using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Queries.GetProjectPartners
{
    public class GetProjectPartnersQueryValidator : AbstractValidator<GetProjectPartnersQuery>
    {
        public GetProjectPartnersQueryValidator()
        {
            RuleFor(p => p.PageId).NotEmpty().Must(c => c != Guid.Empty);
        }
    }
}