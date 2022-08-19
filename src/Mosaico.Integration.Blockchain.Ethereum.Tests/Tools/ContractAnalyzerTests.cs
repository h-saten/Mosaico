using System;
using System.IO;
using System.Threading.Tasks;
using Moq;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Integration.Blockchain.Ethereum.Models;
using Mosaico.Integration.Blockchain.Ethereum.Tools;
using NUnit.Framework;

namespace Mosaico.Integration.Blockchain.Ethereum.Tests.Tools
{
    public class ContractAnalyzerTests
    {

        private string SourceCode;
        private Mock<IEtherScanner> EtherScanner;
        
        [OneTimeSetUp]
        public async Task ReadABI()
        {
            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            SourceCode = await File.ReadAllTextAsync(Path.Combine(projectDirectory, "Tools", "ABI.txt"));
            
            EtherScanner = new Mock<IEtherScanner>();
            EtherScanner.Setup(x => x.TokenContractAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new ContractSourceCodeResponse
                {
                    Message = "",
                    Result = SourceCode,
                    Status = "1"
                });
        }
        
        [Test]
        public async Task ShouldFindAllPassedFunctions()
        {
            // arrange
            var sut = new ContractAnalyzer(EtherScanner.Object);

            // act
            var result = await sut.ContainAllFunctionsAsync("", "", new []{"burn", "mint"});
            
            // assert
           Assert.IsTrue(result);
        }
        
        [Test]
        public async Task ShouldReturnFunctionsAsExisting()
        {
            // arrange
            var sut = new ContractAnalyzer(EtherScanner.Object);

            // act
            var result = await sut.FunctionsExistsAsync("", "", new []{"burn", "mint"});
            
            // assert
           foreach (var functionExistence in result)
           {
               Assert.IsTrue(functionExistence.Value);
           }
        }
        /*
        // TODO for 'manual' testing
        [Test]
        public async Task ShouldReturnFunctionsAsExistingWhenPassedValidAPIConnectionCOnfiguration()
        {
            var ethClientFactory = new Mock<IEthereumClientFactory>();
            ethClientFactory.Setup(x => x.GetConfiguration(It.IsAny<string>()))
                .Returns(new EthereumNetworkConfiguration(
                {
                    
                }));
            
            var etherScanner = new EtherScanner(ethClientFactory.Object);
            // arrange
            var sut = new ContractAnalyzer(etherScanner);

            // act
            var result = await sut.FunctionsExistsAsync("", "", new []{"burn", "mint"});
            
            // assert
           foreach (var functionExistence in result)
           {
               Assert.IsTrue(functionExistence.Value);
           }
        }
        */
    }
}