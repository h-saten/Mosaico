# How to manage VPN
- Currently we use VPN Gateways on Azure to set up VPN.
- You can download VPN client from (HOME -> Virtual network gateways -> vgw-vpnhub-shared -> Point-to-site configuration -> Download VPN Client)
- Then you can use AZURE VPN CLIENT: `https://www.microsoft.com/pl-pl/p/azure-vpn-client/9np355qt2sqb?activetab=pivot:overviewtab`
- You can import there vpn config downloaded from point to site config, and use that config to propagate to users, so they can use our VPN.

# VPN Networking
- Many of terraform modules use vpn gateway. (ACR, AKS, Storage accounts, MSSQL database)
- It is done that way, because we have set up many private endpoints for said Azure modules, to ensure safety, and separation from public network and our internal network.
- Clients get IP address, from subnet pool: `10.2.0.0/16`

## VPN, virtual network peering
- We use virtual network peering to peer, from internal service network - eg. AKS, to a remote vnet-vpnhub-shared (which is in another subscription)
- Every enviroment (dev,test,prod) peers to vnet-vpnhub-share, so it gets access to VPN, that you can access from `Azure VPN client`