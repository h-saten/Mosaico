terraform { #provider block, to use vnet peering from: main vnet -> shared vnet, and vice versa.
  required_providers {
    azurerm = {
      source                = "hashicorp/azurerm"
      version               = "2.88.1"   #latest 2.88.1
      configuration_aliases = [azurerm.shared]
    }
  }
}

resource "azurerm_virtual_network" "vnet" {
  name                = "vnet-mosaico-${var.env}"
  location            = var.location
  resource_group_name = var.resource_group
  address_space       = ["172.${var.network_prefix}.0.0/16"]
}

resource "azurerm_subnet" "default" {

  depends_on = [
    azurerm_virtual_network.vnet
  ]

  name                                           = "snet-default"
  resource_group_name                            = var.resource_group
  virtual_network_name                           = azurerm_virtual_network.vnet.name
  address_prefixes                               = ["172.${var.network_prefix}.16.0/24"]
  enforce_private_link_endpoint_network_policies = true
  enforce_private_link_service_network_policies  = true
  service_endpoints                              = ["Microsoft.ServiceBus", "Microsoft.Sql","Microsoft.KeyVault"]
}

resource "azurerm_subnet" "gateway" {

  depends_on = [
    azurerm_virtual_network.vnet
  ]

  name                                           = "snet-gateway"
  resource_group_name                            = var.resource_group
  virtual_network_name                           = azurerm_virtual_network.vnet.name
  address_prefixes                               = ["172.${var.network_prefix}.17.0/24"]
  enforce_private_link_endpoint_network_policies = true
  enforce_private_link_service_network_policies  = true

}

resource "azurerm_subnet" "aks" {

  depends_on = [
    azurerm_virtual_network.vnet
  ]

  name                                           = "snet-aks"
  resource_group_name                            = var.resource_group
  virtual_network_name                           = azurerm_virtual_network.vnet.name
  address_prefixes                               = ["172.${var.network_prefix}.0.0/20"] #storage account is in this vm with k8s, so no service endpoint
  enforce_private_link_endpoint_network_policies = true
  enforce_private_link_service_network_policies  = true
  service_endpoints                              = ["Microsoft.ServiceBus","Microsoft.Storage"]

}

resource "azurerm_subnet" "aks-virtual-nodes" {

  depends_on = [
    azurerm_virtual_network.vnet
  ]

  name                                           = "snet-aks-virtual-nodes"
  resource_group_name                            = var.resource_group
  virtual_network_name                           = azurerm_virtual_network.vnet.name
  address_prefixes                               = ["172.${var.network_prefix}.18.0/24"]
  enforce_private_link_endpoint_network_policies = true
  enforce_private_link_service_network_policies  = true
  service_endpoints                              = ["Microsoft.ServiceBus"]


}

resource "azurerm_virtual_network_peering" "vn_default_peer" { #Peering to remote vnet
  name                      = "default-to-vnet-vpnhub"         #set up suffix ${var.env} at end
  resource_group_name       = var.resource_group
  virtual_network_name      = azurerm_virtual_network.vnet.name
  remote_virtual_network_id = var.shared_peer_network_id
  allow_gateway_transit     = true
  use_remote_gateways       = true
}

resource "azurerm_virtual_network_peering" "vnet_hub_peer" { #Here, we peer back, from shared vnet to our main vnet.
  provider                  = azurerm.shared
  name                      = "vnet-vpnhub-to-defualt" #set up suffix ${var.env} at end 
  resource_group_name       = var.shared_rg_name
  virtual_network_name      = var.shared_vnet_network_name
  remote_virtual_network_id = azurerm_virtual_network.vnet.id
  allow_gateway_transit     = true
}

data "azurerm_subnet" "shared_subnet" { #used for output block in outputs.tf in this module dir.
  provider             = azurerm.shared
  name                 = "default"
  virtual_network_name = var.shared_vnet_network_name
  resource_group_name  = var.shared_rg_name
}

data "azurerm_subnet" "shared_gateway_subnet" { #used for output block in outputs.tf in this module dir.
  provider             = azurerm.shared
  name                 = "GatewaySubnet"
  virtual_network_name = var.shared_vnet_network_name
  resource_group_name  = var.shared_rg_name
}