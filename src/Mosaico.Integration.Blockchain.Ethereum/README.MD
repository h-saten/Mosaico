# Generating smart contracts

You need:
- visual studio code
- installed solidity extension https://marketplace.visualstudio.com/items?itemName=JuanBlanco.solidity


Create `Contracts` folder in your project. Create a smart contract (Solidity file) there. 
Open it with Visual Studio code. If there is no nethereum-gen.settings file in `Contracts` folder, press F1 in VSC and run action `Solidity: Create 'nethereum-gen.settings' with default values at root`. Open newly generate json file and fill in the information about current project:
```json
{
    "projectName": "Mosaico.Integration.Blockchain.Ethereum", //client library / project name
    "namespace": "Mosaico.Integration.Blockchain.Ethereum.Contracts", //namespace where contracts will be generated
    "lang": 0,
    "autoCodeGen": true,
    "projectPath": "../"
}
```

After that you need to trigger compilation of smart contract. Open solidity file and press F1. Now you need to run command `Solidity: Compile Contract`. After that your your C# smart contract bridge will be automatically generated and placed in the project. 
