using AutoMapper;
using Mosaico.Domain.BusinessManagement.Entities;
using Mosaico.SDK.BusinessManagement.Models;

namespace Mosaico.SDK.BusinessManagement
{
    public class BusinessManagementClientMapperProfile : Profile
    {
        public BusinessManagementClientMapperProfile()
        {
            CreateMap<Company, MosaicoCompany>()
                .ForMember(p => p.Id, opt => opt.MapFrom(c => c.Id))
                .ForMember(p => p.Name, opt => opt.MapFrom(c => c.CompanyName))
                .ForMember(p => p.Email, opt => opt.MapFrom(c => c.Email))
                .ForMember(p => p.PhoneNumber, opt => opt.MapFrom(c => c.PhoneNumber))
                .ForMember(p => p.Country, opt => opt.MapFrom(c => c.Country))
                .ForMember(p => p.Street, opt => opt.MapFrom(c => c.Street))
                .ForMember(p => p.PostalCode, opt => opt.MapFrom(c => c.PostalCode))
                .ForMember(p => p.Region, opt => opt.MapFrom(c => c.Region))
                .ForMember(p => p.IsApproved, opt => opt.MapFrom(c => c.IsApproved))
                .ForMember(p => p.CreatedById, opt => opt.MapFrom(c => c.CreatedById))
                .ForMember(p => p.Network, opt => opt.MapFrom(c => c.Network))
                .ForMember(p => p.ContractAddress, opt => opt.MapFrom(c => c.ContractAddress))
                .ForMember(p => p.IsVotingEnabled, opt => opt.MapFrom(c => c.IsVotingEnabled))
                .ForMember(p => p.OnlyOwnerProposals, opt => opt.MapFrom(c => c.OnlyOwnerProposals))
                .ForMember(p => p.InitialVotingDelay, opt => opt.MapFrom(c => c.InitialVotingDelay))
                .ForMember(p => p.InitialVotingPeriod, opt => opt.MapFrom(c => c.InitialVotingPeriod))
                .ForMember(p => p.Quorum, opt => opt.MapFrom(c => c.Quorum))
                .ForMember(p => p.Slug, opt => opt.MapFrom(c => c.Slug))
                .ForAllOtherMembers(opt => opt.Ignore());

        }
    }
}
