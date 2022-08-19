using FluentValidation;
using Mosaico.Application.BusinessManagement.Queries.GetInvitations;

namespace Mosaico.Application.BusinessManagement.Queries.GetInvitations
{
    public class GetInvitationsQueryValidator : AbstractValidator<GetInvitationsQuery>
    {
        public GetInvitationsQueryValidator()
        {
        }
    }
}