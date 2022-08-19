using System;
using Autofac;
using FluentValidation;
using MediatR.Extensions.Autofac.DependencyInjection;
using Mosaico.Application.Features.Commands.AddBetaTester;
using Mosaico.Application.Features.Commands.UpdateBetaTester;
using Mosaico.Domain.Features;
using Mosaico.Events.Base;
using Mosaico.SDK.Wallet;

namespace Mosaico.Application.Features
{
    public class FeaturesApplicationModule : Module
    {
        public static readonly Type MappingProfileType = typeof(FeaturesMapperProfile);
        
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterMediatR(ThisAssembly);
            //modules
            builder.RegisterModule<FeaturesModule>();
            //validators
            builder.RegisterType<AddBetaTesterCommandValidator>().As<IValidator<AddBetaTesterCommand>>();
            builder.RegisterType<UpdateBetaTesterCommandValidator>().As<IValidator<UpdateBetaTesterCommand>>();
            //event handlers
            //other
        }
    }
}