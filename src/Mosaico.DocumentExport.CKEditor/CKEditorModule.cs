using Autofac;
using Microsoft.Extensions.Configuration;
using Mosaico.DocumentExport.Base;
using Mosaico.DocumentExport.CKEditor.Configurations;

namespace Mosaico.DocumentExport.CKEditor
{
    public class CKEditorModule : Module
    {
        private readonly IConfiguration _configuration;

        public CKEditorModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            var settings = new CKEditorConfiguration();
            _configuration.GetSection(CKEditorConfiguration.SectionName).Bind(settings);
            builder.RegisterInstance(settings).AsSelf();
            builder.RegisterType<CKEditorClient>().As<IDocumentExportClient>();
        }
    }
}