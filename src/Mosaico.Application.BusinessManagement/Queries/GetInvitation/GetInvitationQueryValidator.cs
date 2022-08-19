using FluentValidation;
using Mosaico.Application.BusinessManagement.Queries.GetInvitation;

namespace Mosaico.Application.BusinessManagement.Queries.GetInvitation
{
    public class GetInvitationQueryValidator : AbstractValidator<GetInvitationQuery>
    {
        public GetInvitationQueryValidator()
        {
        }
    }
}