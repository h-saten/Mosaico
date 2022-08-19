using System.Threading.Tasks;
using Moq;
using Mosaico.Base.Abstractions;
using RestSharp;

namespace Mosaico.Integration.Email.EmailLabs.Tests.Helpers
{
    public class TestRestClientInterceptor : IRestClientInterceptor
    {
        public Mock<IRestClient> Client { get; set; }

        public TestRestClientInterceptor()
        {
            Client = new Mock<IRestClient>();
        }
        
        public Task<IRestClient> InterceptAsync(IRestClient client)
        {
            return Task.FromResult(Client.Object);
        }
    }
}