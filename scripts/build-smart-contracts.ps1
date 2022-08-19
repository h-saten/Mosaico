Write-Host 'Compiling smart contracts'
Set-Location -Path $PSScriptRoot/../smart_contracts
truffle compile
Write-Host 'Copying contracts to frontend'
Copy-Item -Path "./build/contracts/*" -Destination "../frontend/mosaico-web-ui/projects/mosaico-wallet/src/lib/smart_contracts/abi/"