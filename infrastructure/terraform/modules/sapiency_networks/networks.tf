resource "azurerm_virtual_network" "vnet" {
  name                = "vnet-sapiency"
  location            = var.location
  resource_group_name = var.resource_group
  address_space       = ["172.${var.network_prefix}.0.0/16"]
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

}

