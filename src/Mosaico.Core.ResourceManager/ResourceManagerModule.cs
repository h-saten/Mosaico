using Autofac;
using Mosaico.Base.Abstractions;

namespace Mosaico.Core.ResourceManager
{
    /*
     * Module that contains registrations for template engine - mustache processor
     */
    public class ResourceManagerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<MustacheTemplateEngine>().As<ITemplateEngine>();
            builder.RegisterType<EmbeddedResourceManager>().As<IResourceManager>();
        }
    }
}