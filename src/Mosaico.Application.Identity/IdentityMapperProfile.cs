using AutoMapper;
using Mosaico.Application.Identity.DTOs;
using Mosaico.Application.Identity.Queries.GetUser;
using Mosaico.Domain.Identity.Entities;

namespace Mosaico.Application.Identity
{
    public class IdentityMapperProfile : Profile
    {
        public IdentityMapperProfile()
        {
            CreateMap<ApplicationUser, GetUserQueryResponse>()
                .ForMember(d => d.IsEmailVerified, opt => opt.MapFrom(u => u.EmailConfirmed))
                .ForMember(d => d.IsAMLVerified, opt => opt.MapFrom(u => u.AMLStatus == AMLStatus.Confirmed || u.AMLStatus == AMLStatus.KangaConfirmed))
                .ForMember(d => d.IsPhoneVerified, opt => opt.MapFrom(u => u.PhoneNumberConfirmed));
            
            CreateMap<UserToPermission, UserPermissionDTO>()
                .ForMember(u => u.Id, opt => opt.MapFrom(p => p.PermissionId))
                .ForMember(u => u.Key, opt => opt.MapFrom(p => p.Permission.Key))
                .ForMember(u => u.EntityId, opt => opt.MapFrom(p => p.EntityId));

            CreateMap<DeletionRequest, DeletionRequestDTO>()
                .ForMember(u => u.Id, opt => opt.MapFrom(p => p.Id))
                .ForMember(u => u.UserId, opt => opt.MapFrom(p => p.UserId))
                .ForMember(u => u.UserName, opt => opt.MapFrom(p => p.User.UserName));
        }
    }
}