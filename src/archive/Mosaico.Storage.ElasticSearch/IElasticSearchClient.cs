using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.Storage.ElasticSearch
{
    public interface IElasticSearchClient
    {
        public Task<IndexResponse> CreateIndex<T>(T value, string index) where T : class;

        public Task<GetResponse<T>> GetDocument<T>(string id, string index) where T : class;

        public Task<IndexResponse> CreateDocumentAndIndex<T>(T document, string index) where T : class;

        public Task<DeleteIndexResponse> DeleteIndex(string index);
    }
}
