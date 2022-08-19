resource "azurerm_user_assigned_identity" "aks_identity" {
  resource_group_name = local.resource_group
  location            = var.location

  name                = "aks-identity"
}

resource "azurerm_user_assigned_identity" "agw_identity" {
  resource_group_name = local.resource_group
  location            = var.location
  
  name                = "agw-identity-${var.env}"
}

resource "azurerm_user_assigned_identity" "agic_identity" {
  resource_group_name = local.resource_group
  location            = var.location
  
  name                = "agic-identity-${var.env}"
}

resource "azurerm_user_assigned_identity" "sb_identity" {
  resource_group_name = local.resource_group
  location            = var.location
  
  name                = "sb-identity-${var.env}"
}

