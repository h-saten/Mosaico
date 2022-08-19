using System;

namespace Mosaico.Integration.SignalR.DTO
{
    public class TokenCreatedDTO
    {
        public Guid TokenId { get; set; }
        public string TokenAddress { get; set; }
    }
}