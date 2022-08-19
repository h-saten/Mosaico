data "azurerm_virtual_network" "vn_shared" {
  provider            = azurerm.shared
  resource_group_name = var.shared_rg_name #set dynamic as default
  name                = var.shared_vnet_network_name
}
