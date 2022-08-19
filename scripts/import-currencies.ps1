Set-Location -Path $PSScriptRoot/../src/Mosaico.Tools.CommandLine
Write-Host '-----------------------BUILDING COMMAND LINE-----------------------'
dotnet build Mosaico.Tools.CommandLine.csproj
Write-Host '-----------------------BUILD FINISHED-----------------------'
Set-Location -Path $PSScriptRoot/../src/Mosaico.Tools.CommandLine/bin/Debug/net5.0
Write-Host '-----------------------IMPORTING CURRENCIES-----------------------'
dotnet Mosaico.Tools.CommandLine.dll import-payment-currency
Set-Location -Path $PSScriptRoot/../scripts
