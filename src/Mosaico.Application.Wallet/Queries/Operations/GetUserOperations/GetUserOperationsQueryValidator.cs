using FluentValidation;

namespace Mosaico.Application.Wallet.Queries.Operations.GetUserOperations
{
    public class GetUserOperationsQueryValidator : AbstractValidator<GetUserOperationsQuery>
    {
        public GetUserOperationsQueryValidator()
        {
            RuleFor(t => t.Skip).GreaterThanOrEqualTo(0);
            RuleFor(t => t.Take).GreaterThan(0);
        }
    }
}