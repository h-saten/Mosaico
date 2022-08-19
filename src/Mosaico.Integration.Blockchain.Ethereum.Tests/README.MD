# Running unit tests
To run unit tests locally, you can install and run Ganache (https://www.trufflesuite.com/ganache). When running for the first time, create a new workspace (don't use quickstart) and connect to the client using Metamask. Then, create a new account and send there 0.2 ETH. Update address of an account in test settings:
```json
"Ethereum": {
    "Endpoint": "HTTP://127.0.0.1:7545", // Ganache localhost
    "AdminAccount": {
      "PrivateKey": "0142f729b106bf594b4cad91ca0f7d543b49691edaa716620c20297eb3d82dc8" // private key of an account (from ganache) which has some ETH
    },
    "Chain": "1337", // ganache local chain ID
    "AdminAccountProviderType": "CONFIGURATION"
  },
  "EthereumTest": {
    "BalanceCheckAddress": "0x2d6763a2348cBC0df05e5838Da63FFD6665e8856" // balance number with 0.2 ETH
  }
```