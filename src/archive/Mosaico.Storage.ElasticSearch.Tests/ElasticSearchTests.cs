using Elasticsearch.Net;
using Moq;
using Mosaico.Storage.ElasticSearch.Configurations;
using Mosaico.Tests.Base;
using Nest;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.Storage.ElasticSearch.Tests
{
    public class ElasticSeachTests : TestBase
    {
        private ElasticSearchConfiguration _config;

        public class ElasticSearchTestClass
        {
            public string Data { get; }

            public ElasticSearchTestClass(string data)
            {
                Data = data;
            }
        }

        [SetUp]
        public void Setup()
        {
            _config = GetSettings<ElasticSearchConfiguration>(ElasticSearchConfiguration.SectionName);
        }

        [Test]
        public void CanCreateElasticClient()
        {
            var connectionPool = new SingleNodeConnectionPool(new Uri(_config.Url));
            var settings = new ConnectionSettings(connectionPool, new InMemoryConnection());

            var elasticClient = new ElasticClient(settings);

            Assert.NotNull(elasticClient);
        }

        [Test]
        public void CanAddIndex()
        {            
            var connectionPool = new SingleNodeConnectionPool(new Uri(_config.Url));
            var connectionSettings = new ConnectionSettings(connectionPool, new InMemoryConnection())
           .DefaultIndex("testindex")
           .PrettyJson()
           .DisableDirectStreaming();

            var client = new ElasticClient(connectionSettings);      
            var request = new SearchRequest("testindex");
            var response = client.Search<ElasticSearchTestClass>(request);
            Assert.IsTrue(response.IsValid);
        }

        [Test]
        public void CanQueryDocuments()
        {
            var searchClass = new List<ElasticSearchTestClass>
            {
                new ElasticSearchTestClass("1"),
                new ElasticSearchTestClass("2"),
            };

            var mockSearchResponse = new Mock<ISearchResponse<ElasticSearchTestClass>>();
            mockSearchResponse.Setup(x => x.Documents).Returns(searchClass);

            var mockElasticClient = new Mock<IElasticClient>();
            mockElasticClient.Setup(x => x
                .Search(It.IsAny<Func<SearchDescriptor<ElasticSearchTestClass>, ISearchRequest>>()))
                .Returns(mockSearchResponse.Object);

            var result = mockElasticClient.Object.Search<ElasticSearchTestClass>(s => s);

            Assert.AreEqual(2, result.Documents.Count());
        }
    }
}