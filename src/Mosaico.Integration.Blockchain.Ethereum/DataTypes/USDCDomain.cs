using System.Numerics;
using Mosaico.Integration.Blockchain.Ethereum.Services.v1;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Signer.EIP712;
using Nethereum.Util;

namespace Mosaico.Integration.Blockchain.Ethereum.DataTypes
{
    [Struct("EIP712Domain")]
    public class USDCDomain : IDomain
    {
        [Parameter("string", "name", 1)]
        public virtual string Name { get; set; }

        [Parameter("string", "version", 2)]
        public virtual string Version { get; set; }
            
        [Parameter("bytes32", "salt", 4)]
        public virtual byte[] Salt { get; set; }
            
        [Parameter("address", "verifyingContract", 3)]
        public virtual string VerifyingContract { get; set; }
        
        public static TypedData<USDCDomain> GetTransferWithAuthorizationType(string contractName, string version, string contract, BigInteger chainId)
        {
            var nameHash = new Sha3Keccack().CalculateHash(contractName);
            var versionHash = new Sha3Keccack().CalculateHash(version);
            return new TypedData<USDCDomain>
            {
                Domain = new USDCDomain
                {
                    Name = nameHash,
                    Version = versionHash,
                    Salt = chainId.ToByteArray(),
                    VerifyingContract = contract
                },
                Types = MemberDescriptionFactory.GetTypesMemberDescription(typeof(USDCDomain), typeof(TransferWithAuthorization)),
                PrimaryType = nameof(TransferWithAuthorization)
            };
        }
    }
}