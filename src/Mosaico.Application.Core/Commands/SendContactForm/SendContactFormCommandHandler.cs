using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Mosaico.Application.Core.Exceptions;
using Mosaico.Integration.UserCom.Abstractions;
using Mosaico.Integration.UserCom.Models.Request;

namespace Mosaico.Application.Core.Commands.SendContactForm
{
    public class SendContactFormCommandHandler : IRequestHandler<SendContactFormCommand>
    {
        private readonly IUserComConversationsClient _conversationsClient; 
        private readonly IUserComUsersApiClient _usersClient; 
        
        public SendContactFormCommandHandler(IUserComConversationsClient userComConversationsClient, IUserComUsersApiClient usersClient)
        {
            _conversationsClient = userComConversationsClient;
            _usersClient = usersClient;
        }

        public async Task<Unit> Handle(SendContactFormCommand request, CancellationToken cancellationToken)
        {
            int userId;
            var user = await _usersClient.FindUserByEmailAsync(request.EmailAddress, cancellationToken);
            if (user is null)
            {
                var createUserResponse = await _usersClient.CreateUserAsync(request.EmailAddress, request.UserName, cancellationToken);
                userId = createUserResponse.Id;
            }
            else
            {
                userId = user.Id;
            }

            if (userId == 0)
            {
                throw new ContactMessageException();
            }
            
            await _conversationsClient.CreateConversationAsync(new CreateConversationParams
            {
                Message = request.Message,
                Name = request.UserName,
                EmailAddress = request.EmailAddress,
                UserId = userId
            }, cancellationToken);

            return Unit.Value;
        }
    }
}