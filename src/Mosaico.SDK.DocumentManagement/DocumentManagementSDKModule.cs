using Autofac;
using Mosaico.SDK.DocumentManagement.Abstractions;

namespace Mosaico.SDK.DocumentManagement
{
    public class DocumentManagementSDKModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<DocumentManagementClient>().As<IDocumentManagementClient>();
        }
        
    }
}