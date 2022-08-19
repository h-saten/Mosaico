using System;
using System.Threading.Tasks;
using Mosaico.Storage.ElasticSearch.Configurations;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using Nest.JsonNetSerializer;
using Elasticsearch.Net;

namespace Mosaico.Storage.ElasticSearch
{
    public class ElasticSearchClient : IElasticSearchClient
    {
        private ElasticClient _client;
        private readonly ElasticSearchConfiguration _config;
        private readonly ILogger _logger;
        public ElasticSearchClient(ElasticSearchConfiguration config, ILogger logger = null)
        {
            _config = config;
            _logger = logger;
        }

        private ElasticClient CreateElasticClient()
        {
            if (_client != null) {
                return _client;
            }

            var pool = new SingleNodeConnectionPool(new Uri(_config.Url));

            var connectionSettings = new ConnectionSettings(pool, sourceSerializer: JsonNetSerializer.Default);

            return _client = new ElasticClient(connectionSettings);
        }

        public async Task<IndexResponse> CreateIndex<T>(T value, string index) where T : class
        {
             _client = CreateElasticClient();
            var response = await _client.IndexAsync(value, idx => idx.Index(index));
            return response;
        }

        public async Task<GetResponse<T>> GetDocument<T>(string id, string index) where T : class
        {
            _client = CreateElasticClient();
            return await _client.GetAsync<T>(id, idx => idx.Index(index));
        }
           
        public async Task<IndexResponse> CreateDocumentAndIndex<T>(T document, string index) where T : class
        {

            _client = CreateElasticClient();

            var serializedObject = JsonConvert.SerializeObject(document, Formatting.None,
                   new JsonSerializerSettings()
                   {
                       ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                   });

            var elasticValues = new ElasticSeachValues
            {
                Mappings = JObject.Parse(serializedObject)
            };

            _logger?.Verbose($"Adding index:{0}", index);

            return await _client.IndexAsync(elasticValues, idx => idx.Index(index.ToLower()));
        }

        public async Task<DeleteIndexResponse> DeleteIndex(string index)
        {
            _client = CreateElasticClient();

            if (_client.Indices.Exists(index.ToLower()).Exists)
            {
                return await _client.Indices.DeleteAsync(index.ToLower());
            }
            return new DeleteIndexResponse();
        }

        public class ElasticSeachValues 
        {
            [JsonProperty("mappings")]
            public JObject Mappings { get; set; }
        }

    }
}