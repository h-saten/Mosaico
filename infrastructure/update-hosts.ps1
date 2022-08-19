if (!([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole] "Administrator")) { Start-Process powershell.exe "-NoProfile -ExecutionPolicy Bypass -File `"$PSCommandPath`"" -Verb RunAs; exit }
Write-Output "Adding mappings for DEV environment"
Add-Content -Path $env:windir\System32\drivers\etc\hosts -Value "`n172.17.0.4 dns-aks-dev-2128f500.61380022-4189-458b-9a03-0892ab631da9.privatelink.westeurope.azmk8s.io" -Force
Add-Content -Path $env:windir\System32\drivers\etc\hosts -Value "`n172.17.16.6 sql-mosaico-dev.database.windows.net" -Force
# Add-Content -Path $env:windir\System32\drivers\etc\hosts -Value "`n172.17.16.5 kv-mosaico-dev.vault.azure.net" -Force
Add-Content -Path $env:windir\System32\drivers\etc\hosts -Value "`n172.19.0.5 dns-aks-prod-9c8dc293.f9419251-d7d1-4576-9558-f3a112d1204b.privatelink.westeurope.azmk8s.io" -Force
