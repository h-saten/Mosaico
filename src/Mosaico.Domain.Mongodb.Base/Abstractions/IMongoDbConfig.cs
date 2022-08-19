using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.Domain.Mongodb.Base.Abstractions
{
    public interface IMongoDbConfig
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
