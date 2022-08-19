& .\recreate-databases.ps1
& .\run-data-seed.ps1
Set-Location -Path $PSScriptRoot/../src/Mosaico.Tools.CommandLine/bin/Debug/net5.0
Write-Host '-----------------------SEED USDT WALLET -----------------------'
dotnet Mosaico.Tools.CommandLine.dll usdt-wallet-topup
dotnet Mosaico.Tools.CommandLine.dll import-payment-currency
Set-Location -Path $PSScriptRoot/../scripts
