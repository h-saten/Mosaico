using Autofac;

namespace Mosaico.Base.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static void AddUrl(this ContainerBuilder builder, string url, string key)
        {
            builder.RegisterInstance(url).Keyed<string>(key);
        }
    }
}