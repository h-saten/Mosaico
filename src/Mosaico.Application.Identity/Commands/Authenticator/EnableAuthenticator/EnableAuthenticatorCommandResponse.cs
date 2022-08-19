using System.Collections.Generic;

namespace Mosaico.Application.Identity.Commands.Authenticator.EnableAuthenticator
{
    public class EnableAuthenticatorCommandResponse
    {
        public List<string> RecoveryCodes { get; set; } = new List<string>();
    }
}