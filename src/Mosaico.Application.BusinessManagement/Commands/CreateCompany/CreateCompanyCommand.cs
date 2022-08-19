using System;
using System.Text.Json.Serialization;
using MediatR;
using Mosaico.Authorization.Base;
using Mosaico.Domain.BusinessManagement.Entities;
using Mosaico.Integration.SignalR.DTO;
using Newtonsoft.Json.Converters;

namespace Mosaico.Application.BusinessManagement.Commands.CreateCompany
{
    [ShouldCompleteEvaluation]
    public class CreateCompanyCommand : IRequest<CompanyCreatedDTO>
    {
        public string CompanyName { get; set; }
        public string CompanyDescription { get; set; }
        public string Country { get; set; }
        public string Street { get; set; }
        public string VATId { get; set; }
        public string PostalCode { get; set; }
        public string Size { get; set; }
        public bool IsVotingEnabled { get; set; }
        public string Network { get; set; }
        public string Region { get; set; }
        public bool OnlyOwnerProposals { get; set; }
        
        [JsonConverter(typeof(StringEnumConverter))]
        public VotingPeriod InitialVotingDelay { get; set; }
        
        [JsonConverter(typeof(StringEnumConverter))]
        public VotingPeriod InitialVotingPeriod { get; set; }
        public long Quorum { get; set; }
    }
}