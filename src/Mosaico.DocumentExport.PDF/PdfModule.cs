using Autofac;
using Microsoft.Extensions.Configuration;
using Mosaico.DocumentExport.PDF.Generator;

namespace Mosaico.DocumentExport.PDF
{
    public class PdfModule : Module
    {
        private readonly IConfiguration _configuration;

        public PdfModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<PdfGenerator>().As<IPdfClient>();
        }
    }
}