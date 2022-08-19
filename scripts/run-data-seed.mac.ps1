Set-Location -Path $PSScriptRoot/../src/Mosaico.Tools.CommandLine
Write-Host '-----------------------BUILDING COMMAND LINE-----------------------'
sudo dotnet build Mosaico.Tools.CommandLine.csproj
Write-Host '-----------------------BUILD FINISHED-----------------------'
Set-Location -Path $PSScriptRoot/../src/Mosaico.Tools.CommandLine/bin/Debug/net5.0
Write-Host '-----------------------SEED USERS, PROJECTS, WALLETS AND TRANSACTIONS -----------------------'
sudo dotnet Mosaico.Tools.CommandLine.dll generate-fake-data