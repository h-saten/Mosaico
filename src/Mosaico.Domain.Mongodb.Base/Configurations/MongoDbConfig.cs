using Mosaico.Domain.Mongodb.Base.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.Domain.Mongodb.Base.Configurations
{
    public class MongoDbConfig : IMongoDbConfig
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
