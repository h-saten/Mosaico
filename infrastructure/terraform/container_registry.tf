data "azurerm_container_registry" "acr" {
  provider            = azurerm.shared3
  name                = var.shared3_acr_name
  resource_group_name = var.shared3_rg_name
}