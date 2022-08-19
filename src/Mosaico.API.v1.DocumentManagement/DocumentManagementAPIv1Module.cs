using Autofac;
using Microsoft.Extensions.Configuration;
using Mosaico.API.v1.DocumentManagement.Settings;
using Mosaico.API.v1.DocumentManagement.Validators;
using Mosaico.Application.DocumentManagement;
using Mosaico.Validation.Base.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.API.v1.DocumentManagement
{
    public class DocumentManagementAPIv1Module : Module
    {
        public static readonly Type MappingProfileType = DocumentManagementApplicationModule.MappingProfileType;

        private readonly IConfiguration _configuration;
        public DocumentManagementAPIv1Module(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterModule<DocumentManagementApplicationModule>();
    
            //configuration
            var apiSettings = new DocumentManagementAPISettings();
            _configuration.GetSection(DocumentManagementAPISettings.SectionName).Bind(apiSettings);
            var validationResult = new DocumentManagementAPISettingsValidator().Validate(apiSettings);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors.FirstOrDefault()?.ErrorMessage ?? Constants.ErrorCodes.DocumentAPIMisconfigured);
            builder.RegisterInstance(apiSettings).AsSelf();
        }
    }
}
