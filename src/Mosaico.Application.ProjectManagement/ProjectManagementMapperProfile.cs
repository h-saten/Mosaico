using AutoMapper;
using Mosaico.Application.ProjectManagement.Commands.CreateProject;
using Mosaico.Application.ProjectManagement.Commands.TokenPage.CreateUpdateProjectPartner;
using Mosaico.Application.ProjectManagement.Commands.TokenPage.CreateUpdateProjectTeamMember;
using Mosaico.Application.ProjectManagement.DTOs;
using Mosaico.Application.ProjectManagement.DTOs.TokenPage;
using Mosaico.Domain.ProjectManagement.Entities;
using Mosaico.Domain.ProjectManagement.Entities.TokenPage;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.SDK.Wallet.Models;

namespace Mosaico.Application.ProjectManagement
{
    public class ProjectManagementMapperProfile : Profile
    {
        public ProjectManagementMapperProfile()
        {
            CreateMap<Project, ProjectDTO>()
                .ForMember(p => p.Status, opt => opt.MapFrom(d => d.Status.Title))
                .ForMember(p => p.IsPublic, opt => opt.MapFrom(p => p.IsAccessibleViaLink))
                .ForMember(p => p.TokenName, opt => opt.Ignore())
                .ForMember(p => p.TokenSymbol, opt => opt.Ignore())
                .ForMember(p => p.HardCap, opt => opt.MapFrom(p => p.Crowdsale != null ? p.Crowdsale.HardCap : 0))
                .ForMember(p => p.SoftCap, opt => opt.MapFrom(p => p.Crowdsale != null ? p.Crowdsale.SoftCap : 0))
                .ForMember(p => p.RaisedCapital, opt => opt.Ignore())
                .ForMember(p => p.ActiveStage, opt => opt.Ignore())
                .ForMember(p => p.IsExchangeAvailable, opt => opt.Ignore());

            CreateMap<Project, ProjectDetailDTO>()
                .ForMember(p => p.Status, opt => opt.MapFrom(d => d.Status.Title))
                .ForMember(p => p.IsPublic, opt => opt.MapFrom(p => p.IsAccessibleViaLink))
                .ForMember(p => p.PaymentMethods,
                    opt => opt.Ignore());

            CreateMap<Stage, StageDTO>()
                .ForMember(s => s.Status, opt => opt.MapFrom(s => s.Status.Title))
                .ForMember(s => s.SoldTokens, opt => opt.Ignore())
                .ForMember(s => s.ProgressPercentage, opt => opt.Ignore());

            CreateMap<StageCreationDTO, Stage>()
                .ForMember(s => s.Name, opt => opt.MapFrom(d => d.Name))
                .ForMember(s => s.EndDate, opt => opt.MapFrom(d => d.EndDate))
                .ForMember(s => s.StartDate, opt => opt.MapFrom(d => d.StartDate))
                .ForMember(s => s.TokenPrice, opt => opt.MapFrom(d => d.TokenPrice))
                .ForMember(s => s.TokensSupply, opt => opt.MapFrom(d => d.TokensSupply))
                .ForMember(s => s.MinimumPurchase, opt => opt.MapFrom(d => d.MinimumPurchase))
                .ForMember(s => s.MaximumPurchase, opt => opt.MapFrom(d => d.MaximumPurchase))
                .ForMember(s => s.Type, opt => opt.MapFrom(d => d.Type))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<CreateProjectCommand, Project>()
                .ForMember(p => p.Title, opt => opt.MapFrom(d => d.Title.Trim()))
                .ForMember(p => p.TitleInvariant, opt => opt.MapFrom(d => d.Title.Trim().ToUpperInvariant()))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<TokenDTO, MosaicoToken>()
                .ForMember(t => t.Name, opt => opt.MapFrom(d => d.Name))
                .ForMember(t => t.Network, opt => opt.MapFrom(d => d.Network))
                .ForMember(t => t.Symbol, opt => opt.MapFrom(d => d.Symbol))
                .ForMember(t => t.TotalSupply, opt => opt.MapFrom(d => d.TotalSupply))
                .ForMember(t => t.IsMintable, opt => opt.MapFrom(d => d.IsMintable))
                .ForMember(t => t.IsBurnable, opt => opt.MapFrom(d => d.IsBurnable))
                .ForMember(t => t.Type, opt => opt.MapFrom(d => d.Type))
                .ForMember(t => t.LogoUrl, opt => opt.MapFrom(d => d.LogoUrl))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<MosaicoToken, TokenDTO>()
                .ForMember(t => t.Name, opt => opt.MapFrom(d => d.Name))
                .ForMember(t => t.Network, opt => opt.MapFrom(d => d.Network))
                .ForMember(t => t.Symbol, opt => opt.MapFrom(d => d.Symbol))
                .ForMember(t => t.TotalSupply, opt => opt.MapFrom(d => d.TotalSupply))
                .ForMember(t => t.IsMintable, opt => opt.MapFrom(d => d.IsMintable))
                .ForMember(t => t.IsBurnable, opt => opt.MapFrom(d => d.IsBurnable))
                .ForMember(t => t.Type, opt => opt.MapFrom(d => d.Type))
                .ForMember(t => t.LogoUrl, opt => opt.MapFrom(d => d.LogoUrl))
                .ForAllOtherMembers(opt => opt.Ignore());
            
            CreateMap<PageTeamMember, ProjectTeamMemberDTO>()
                .ForMember(v => v.PageId, opt => opt.MapFrom(d => d.PageId))
                .ForMember(v => v.Id, opt => opt.MapFrom(d => d.Id))
                .ForMember(v => v.Name, opt => opt.MapFrom(d => d.Name))
                .ForMember(v => v.Position, opt => opt.MapFrom(d => d.Position))
                .ForMember(v => v.Facebook, opt => opt.MapFrom(d => d.Facebook))
                .ForMember(v => v.Twitter, opt => opt.MapFrom(d => d.Twitter))
                .ForMember(v => v.LinkedIn, opt => opt.MapFrom(d => d.LinkedIn))
                .ForMember(v => v.PhotoUrl, opt => opt.MapFrom(d => d.PhotoUrl))
                .ForMember(v => v.Order, opt => opt.MapFrom(d => d.Order));

            CreateMap<CreateUpdateProjectTeamMemberCommand, PageTeamMember>().ReverseMap();

            CreateMap<ProjectRole, ProjectRoleDTO>();
            CreateMap<ProjectMember, ProjectMemberDTO>();
            CreateMap<Project, UpdateProjectDTO>();
            CreateMap<UpdateProjectDTO, Project>()
                .ForMember(v => v.Title, opt => opt.MapFrom(s => s.Title))
                .ForMember(v => v.TitleInvariant, opt => opt.MapFrom(s => s.Title.Trim().ToUpperInvariant()))
                .ForMember(v => v.TokenId, opt => opt.MapFrom(s => s.TokenId))
                .ForAllOtherMembers(opt => opt.Ignore());

            //partners
            CreateMap<PagePartners, ProjectPartnerDTO>()
                .ForMember(v => v.PageId, opt => opt.MapFrom(d => d.PageId))
                .ForMember(v => v.Id, opt => opt.MapFrom(d => d.Id))
                .ForMember(v => v.Name, opt => opt.MapFrom(d => d.Name))
                .ForMember(v => v.Position, opt => opt.MapFrom(d => d.Position))
                .ForMember(v => v.Facebook, opt => opt.MapFrom(d => d.Facebook))
                .ForMember(v => v.Twitter, opt => opt.MapFrom(d => d.Twitter))
                .ForMember(v => v.LinkedIn, opt => opt.MapFrom(d => d.LinkedIn))
                .ForMember(v => v.PhotoUrl, opt => opt.MapFrom(d => d.PhotoUrl))
                .ForMember(v => v.Order, opt => opt.MapFrom(d => d.Order));
            CreateMap<CreateUpdateProjectPartnerCommand, PagePartners>().ReverseMap();

            CreateMap<Script, ScriptDTO>();
            CreateMap<DocumentType, DocumentTypesDTO>();
            CreateMap<DocumentTemplate, DocumentTemplateDTO>();
            CreateMap<About, AboutDTO>();
            CreateMap<Article, ArticleDTO>();
            CreateMap<PageReview, PageReviewDTO>();
            CreateMap<ProjectNewsletterSubscription, ProjectSubscriberDTO>()
                .ForMember(t => t.Name, opt => opt.Ignore());
        }
    }
}