using System.Threading.Tasks;
using Mosaico.Core.ResourceManager.Exceptions;
using Mosaico.Tests.Base;
using NUnit.Framework;

namespace Mosaico.Core.ResourceManager.Tests
{
    [TestFixture]
    public class EmbeddedResourceManagerTests : TestBase
    {
        [Test]
        public async Task ShouldReturnResourceBytes()
        {
            //Arrange
            var resourceManager = new EmbeddedResourceManager();
            //Act
            var res = await resourceManager.GetResourceAsync("Images", "logo_mosaico.png", assembly: typeof(EmbeddedResourceManagerTests).Assembly);
            //Assert
            Assert.AreEqual(6042, res.Length);
        }

        [Test]
        public async Task ShouldReturnResourceText()
        {
            //Arrange
            const string fileContent = "Hello World";
            var resourceManager = new EmbeddedResourceManager();
            //Act
            var res = await resourceManager.GetTextResourceAsync("Texts", "Test", assembly: typeof(EmbeddedResourceManagerTests).Assembly);
            //Assert
            Assert.AreEqual(fileContent, res);
        }

        [Test]
        public void ShouldFailIfNoCategory()
        {
            //Arrange
            var resourceManager = new EmbeddedResourceManager();
            //Act
            Assert.ThrowsAsync<ResourceCategoryNotFoundException>(async () =>
            {
                await resourceManager.GetTextResourceAsync("Media", "Test", assembly: typeof(EmbeddedResourceManagerTests).Assembly);
            });
        }
        
        [Test]
        public void ShouldFailIfNoResource()
        {
            //Arrange
            var resourceManager = new EmbeddedResourceManager();
            //Act
            Assert.ThrowsAsync<ResourceNotFoundException>(async () =>
            {
                await resourceManager.GetTextResourceAsync("Texts", "Test2", assembly: typeof(EmbeddedResourceManagerTests).Assembly);
            });
        }
    }
}