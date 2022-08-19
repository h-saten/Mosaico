using System;
using Autofac;
using MediatR.Extensions.Autofac.DependencyInjection;
using Mosaico.Application.DocumentManagement.Abstractions;
using Mosaico.Application.DocumentManagement.EventHandlers;
using Mosaico.Application.DocumentManagement.Services;
using Mosaico.Events.Base;
using Mosaico.Validation.Base.Extensions;

namespace Mosaico.Application.DocumentManagement
{
    public class DocumentManagementApplicationModule : Module
    {
        public static readonly Type MappingProfileType = typeof(DocumentManagementMapperProfile);
        
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterMediatR(ThisAssembly);
            builder.RegisterAssemblyValidators(ThisAssembly);
            //EventHandlers
            builder.RegisterType<ConvertOnProjectDocumentUpdated>().As<IEventHandler>();
            builder.RegisterType<UpdatePicturesOnArticleUpdated>().As<IEventHandler>();
            builder.RegisterType<DocumentUploadService>().As<IDocumentUploadService>();
        }
    }
}