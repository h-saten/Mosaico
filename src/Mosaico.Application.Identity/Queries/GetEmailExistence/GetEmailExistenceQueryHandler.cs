using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Mosaico.Domain.Identity.Entities;

namespace Mosaico.Application.Identity.Queries.GetEmailExistence
{
    public class GetEmailExistenceQueryHandler : IRequestHandler<GetEmailExistenceQuery, GetEmailExistenceQueryResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public GetEmailExistenceQueryHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<GetEmailExistenceQueryResponse> Handle(GetEmailExistenceQuery request, CancellationToken cancellationToken)
        {
            var email = request.Email.Trim().Replace(" ", "+");
            var user = await _userManager.FindByEmailAsync(email);
            return new GetEmailExistenceQueryResponse
            {
                Exist = user != null
            };
        }
    }
}