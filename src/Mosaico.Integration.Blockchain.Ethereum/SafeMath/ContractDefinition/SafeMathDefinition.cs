using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts;
using System.Threading;

namespace Mosaico.Integration.Blockchain.Ethereum.SafeMath.ContractDefinition
{


    public partial class SafeMathDeployment : SafeMathDeploymentBase
    {
        public SafeMathDeployment() : base(BYTECODE) { }
        public SafeMathDeployment(string byteCode) : base(byteCode) { }
    }

    public class SafeMathDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "60566037600b82828239805160001a607314602a57634e487b7160e01b600052600060045260246000fd5b30600052607381538281f3fe73000000000000000000000000000000000000000030146080604052600080fdfea2646970667358221220ea4d3fac1a598ed894a759b7ad8209202ce2e508886fbb5c09ee099f9a0bc4ec64736f6c634300080d0033";
        public SafeMathDeploymentBase() : base(BYTECODE) { }
        public SafeMathDeploymentBase(string byteCode) : base(byteCode) { }

    }
}
