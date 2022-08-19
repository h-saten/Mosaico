using Autofac;
using Microsoft.Extensions.Configuration;
using Mosaico.Storage.ElasticSearch.Configurations;
using Nest;

namespace Mosaico.Storage.ElasticSearch
{
    public class ElasticSearchModule : Module
    {
       
        private readonly IConfiguration _configuration;

        public ElasticSearchModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            var elasticSearchSettings = new ElasticSearchConfiguration();
            _configuration.GetSection(ElasticSearchConfiguration.SectionName).Bind(elasticSearchSettings);


            builder.RegisterInstance(elasticSearchSettings);
            builder.RegisterType<ElasticSearchClient>().As<IElasticSearchClient>();
        }
    }
}