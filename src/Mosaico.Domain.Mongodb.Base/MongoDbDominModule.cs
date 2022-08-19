using Autofac;
using Microsoft.Extensions.Configuration;
using Mosaico.Domain.Mongodb.Base.Abstractions;
using Mosaico.Domain.Mongodb.Base.Configurations;
using Mosaico.Domain.Mongodb.Base.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.Domain.Mongodb.Base
{
    public class MongoDbDominModule : Module
    {
        private readonly IConfiguration _configuration;
        private readonly string _configurationSectionName;

        public MongoDbDominModule(IConfiguration configuration, string configurationSectionName = Mosaico.Domain.Mongodb.Base.Constants.SectionName)
        {
            _configuration = configuration;
            _configurationSectionName = configurationSectionName;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            var mongoDbSettings = new MongoDbConfig();
            _configuration.GetSection(_configurationSectionName).Bind(mongoDbSettings);
            builder.RegisterInstance(mongoDbSettings);
            builder.RegisterType<MongoDBContext>().As<IMongoDBContext>();
        }
    }
}
