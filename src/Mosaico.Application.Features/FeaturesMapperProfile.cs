using System.Linq;
using AutoMapper;
using Mosaico.Application.Features.DTOs;
using Mosaico.Application.Features.Queries.GetSetting;
using Mosaico.Domain.Features.Entities;

namespace Mosaico.Application.Features
{
    public class FeaturesMapperProfile : Profile
    {
        public FeaturesMapperProfile()
        {
            CreateMap<Feature, FeatureDTO>();
            CreateMap<Feature, GetSettingQueryResponse>();

        }
    }
}