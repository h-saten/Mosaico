using AutoMapper;
using Mosaico.Application.BusinessManagement.Commands.CreateCompany;
using Mosaico.Application.BusinessManagement.Commands.CreateCompanyTeamMember;
using Mosaico.Application.BusinessManagement.Commands.CreateVerification;
using Mosaico.Application.BusinessManagement.Commands.UpdateCompany;
using Mosaico.Application.BusinessManagement.DTOs;
using Mosaico.Domain.BusinessManagement.Entities;

namespace Mosaico.Application.BusinessManagement
{
    public class BusinessManagementMapperProfile : Profile
    {
        public BusinessManagementMapperProfile()
        {
            CreateMap<Company, CompanyDTO>();
            CreateMap<Company, CompanyListDTO>()
                .ForMember(c => c.TotalProposals, opt => opt.Ignore())
                .ForMember(c => c.OpenProposals, opt => opt.Ignore())
                .ForMember(c => c.IsSubscribed, opt => opt.Ignore())
                .ForMember(c => c.Projects, opt => opt.Ignore());
            
            CreateMap<TeamMember, TeamMemberDTO>().ReverseMap();

            CreateMap<CreateCompanyCommand, Company>()
                .ForMember(p => p.CompanyName, opt => opt.MapFrom(d => d.CompanyName))
                .ForMember(p => p.CompanyDescription,opt => opt.MapFrom(d => d.CompanyDescription))
                .ForMember(p => p.Country, opt => opt.MapFrom(d => d.Country))
                .ForMember(p => p.PostalCode, opt => opt.MapFrom(d => d.PostalCode))
                .ForMember(p => p.Street, opt => opt.MapFrom(d => d.Street))
                .ForMember(p => p.VATId, opt => opt.MapFrom(d => d.VATId))
                .ForMember(p=> p.Size, opt => opt.MapFrom(d => d.Size))
                .ForMember(p=> p.IsVotingEnabled, opt => opt.MapFrom(d => d.IsVotingEnabled))
                .ForMember(p=> p.OnlyOwnerProposals, opt => opt.MapFrom(d => d.OnlyOwnerProposals))
                .ForMember(p=> p.Region, opt => opt.MapFrom(d => d.Region))
                .ForMember(p=> p.Network, opt => opt.MapFrom(d => d.Network))
                .ForMember(p=> p.InitialVotingDelay, opt => opt.MapFrom(d => d.InitialVotingDelay))
                .ForMember(p=> p.InitialVotingPeriod, opt => opt.MapFrom(d => d.InitialVotingPeriod))
                .ForMember(p=> p.Quorum, opt => opt.MapFrom(d => d.Quorum))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<UpdateCompanyCommand, Company>()
                .ForMember(p => p.CompanyDescription, opt => opt.MapFrom(d => d.CompanyDescription))
                .ForMember(p => p.Country, opt => opt.MapFrom(d => d.Country))
                .ForMember(p => p.PostalCode, opt => opt.MapFrom(d => d.PostalCode))
                .ForMember(p => p.Street, opt => opt.MapFrom(d => d.Street))
                .ForMember(p => p.VATId, opt => opt.MapFrom(d => d.VATId))
                .ForMember(p => p.Size, opt => opt.MapFrom(d => d.Size))
                .ForMember(p => p.Region, opt => opt.MapFrom(d => d.Region))
                .ForMember(p => p.PhoneNumber, opt => opt.MapFrom(d => d.PhoneNumber))
                .ForMember(p => p.Email, opt => opt.MapFrom(d => d.Email))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<CreateTeamMemberCommand, TeamMember>().ReverseMap();
            CreateMap<CreateVerificationCommand, Verification>()
                .ForMember(p => p.CompanyAddressUrl, opt => opt.MapFrom(d => d.CompanyAddressUrl))
                .ForMember(p => p.CompanyRegistrationUrl, opt => opt.MapFrom(d => d.CompanyRegistrationUrl))
                .ForMember(p => p.CompanyId, opt => opt.MapFrom(d => d.CompanyId))
                .ForAllOtherMembers(opt => opt.Ignore());
            CreateMap<Verification, VerificationDTO>()
                .ForMember(p=>p.CompanyAddressUrl, opt => opt.MapFrom(d=>d.CompanyAddressUrl))
                .ForMember(p => p.CompanyRegistrationUrl, opt => opt.MapFrom(d => d.CompanyRegistrationUrl))
                .ForMember(p => p.CompanyId, opt => opt.MapFrom(d => d.CompanyId))
                .ForAllOtherMembers(opt => opt.Ignore());
            CreateMap<Shareholder, ShareholderDTO>().ReverseMap();

            CreateMap<Proposal, ProposalDTO>()
                .ForMember(t => t.AbstainCount, opt => opt.Ignore())
                .ForMember(t => t.AgainstCount, opt => opt.Ignore())
                .ForMember(t => t.ForCount, opt => opt.Ignore())
                .ForMember(t => t.QuorumReached, opt => opt.Ignore())
                .ForMember(t => t.Status, opt => opt.Ignore())
                .ForMember(t => t.VoteCount, opt => opt.Ignore());
        }
    }
}