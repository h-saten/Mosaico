using AutoMapper;
using Mosaico.Domain.ProjectManagement.Entities;
using Mosaico.Domain.ProjectManagement.Entities.Affiliation;
using Mosaico.SDK.ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.SDK.ProjectManagement
{
    public class ProjectManagementClientMapperProfile : Profile
    {
        public ProjectManagementClientMapperProfile()
        {
            CreateMap<StagePurchaseLimit, ProjectStageLimit>();
            CreateMap<Stage, ProjectStage>()
                .ForMember(x => x.IsPrivate, opt => opt.MapFrom(opt => (opt.Type == StageType.Private ? true : false)))
                .ForMember(x => x.IsPreSale, opt => opt.MapFrom(opt => (opt.Type == StageType.PreSale ? true : false)))
                .ForMember(x => x.Status, opt => opt.MapFrom(opt => opt.Status.Key));
            CreateMap<Project, MosaicoProject>();

            CreateMap<AirdropParticipant, MosaicoAirdropParticipant>();
            CreateMap<AirdropCampaign, MosaicoAirdrop>();
            CreateMap<ProjectInvestor, MosaicoProjectInvestor>();
            CreateMap<Project, MosaicoProjectDetails>()
                .ForMember(x => x.CrowdsaleContractAddress, opt => opt.MapFrom(opt => (opt.Crowdsale == null ? null : opt.Crowdsale.ContractAddress)))
                .ForMember(x => x.CrowdsaleContractVersion, opt => opt.MapFrom(opt => (opt.Crowdsale == null ? null : opt.Crowdsale.ContractVersion)))
                .ForMember(x => x.HardCap, opt => opt.MapFrom(opt => (opt.Crowdsale == null ? 0 : opt.Crowdsale.HardCap)))
                .ForMember(x => x.SoftCap, opt => opt.MapFrom(opt => (opt.Crowdsale == null ? 0 : opt.Crowdsale.SoftCap)))
                .ForMember(x => x.CrowdsaleOwnerAddress, opt => opt.MapFrom(opt => (opt.Crowdsale == null ? null : opt.Crowdsale.OwnerAddress)))
                .ForMember(x => x.SaleInProgress, opt => opt.MapFrom(opt => (opt.Status.Key == Domain.ProjectManagement.Constants.ProjectStatuses.InProgress ? true : false)))
                .ForMember(X => X.Network, opt => opt.MapFrom(opt => (opt.Crowdsale == null ? null : opt.Crowdsale.Network)))
                .ForMember(x => x.PaymentMethods, opt => opt.MapFrom(opt => (opt.PaymentMethods.Select(x => x.Key).ToList())));
            CreateMap<Project, MosaicoProjectTransaction>()
                .ForMember(x=>x.ProjectName, opt => opt.MapFrom(opt => opt.Title));
            CreateMap<Document, MosaicoProjectDocument>();
        }
    }
}
