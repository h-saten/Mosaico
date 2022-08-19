resource "azurerm_storage_account" "storage" {
  lifecycle {
    prevent_destroy = true
    ignore_changes = all
  }
  name                     = "stmosaicoterraform"
  resource_group_name      = var.resource_group
  location                 = var.location
  account_tier             = "Standard"
  account_replication_type = "GRS"
  min_tls_version           = "TLS1_2"
  access_tier               = "Hot"
  account_kind              = "StorageV2"
  allow_blob_public_access  = true
}

resource "azurerm_role_definition" "mosaico_terraformrule1" {
  lifecycle {
    prevent_destroy = true
    ignore_changes = all
  }
  name        = "storage_terraform_rule_${var.env}"
  scope       = "/subscriptions/${var.subscription_id}/resourceGroups/${var.resource_group}"
  description = "Role to access storage account from Pay as you go  Subscription"

  permissions {
    actions     = ["Microsoft.Storage/storageAccounts/listKeys/action"] #we need that action to properly export database as a backup
  }
  #assigning scope where service bus itself residses, and where our shared vnet is placed.
  assignable_scopes = [
    "/subscriptions/${var.subscription_id}/resourceGroups/${var.resource_group}"
  ]
}

resource "azurerm_role_assignment" "storage_account_role_assignment" {
  scope              = "/subscriptions/${var.subscription_id}/resourceGroups/${var.resource_group}"
  role_definition_id = azurerm_role_definition.mosaico_terraformrule1.role_definition_resource_id
  principal_id       = var.service_principal_id
}

resource "azurerm_role_assignment" "storage_account_role_assignment2" {
  scope                = "/subscriptions/${var.subscription_id}/resourceGroups/${var.resource_group}/providers/Microsoft.Storage/storageAccounts/${azurerm_storage_account.storage.name}"
  role_definition_name = "Owner"
  principal_id         = var.service_principal_id
}

resource "azurerm_role_assignment" "storage_account_role_assignment4" {
  scope                = "/subscriptions/${var.subscription_id}/resourceGroups/${var.resource_group}/providers/Microsoft.Storage/storageAccounts/${azurerm_storage_account.storage.name}"
  role_definition_name = "Storage Blob Data Reader"
  principal_id         = var.service_principal_id
}

resource "azurerm_role_assignment" "storage_account_role_assignment5" {
  scope                = "/subscriptions/${var.subscription_id}/resourceGroups/${var.resource_group}/providers/Microsoft.Storage/storageAccounts/${azurerm_storage_account.storage.name}"
  role_definition_name = "Storage Blob Data Owner"
  principal_id         = var.service_principal_id
}

resource "azurerm_role_assignment" "storage_account_role_assignment6" {
  scope                = "/subscriptions/${var.subscription_id}/resourceGroups/${var.resource_group}/providers/Microsoft.Storage/storageAccounts/${azurerm_storage_account.storage.name}"
  role_definition_name = "Storage Blob Data Contributor"
  principal_id         = var.service_principal_id
}

resource "azurerm_private_endpoint" "private_endpoint" {
  lifecycle {
    prevent_destroy = true
    ignore_changes = all
  }
  name                = "pe-mosaico-st-terraform${var.env}" #private endpoint
  location            = var.location
  resource_group_name = var.resource_group
  subnet_id           = var.vnet_subnet_id #probably should be snet aks

  private_dns_zone_group {
    name                 = "private-dns-zone-group"
    private_dns_zone_ids = [var.dns_zone_id]
  }

  private_service_connection {
    name                           = "psc-st-terraform-mosaico${var.env}"
    is_manual_connection           = false
    private_connection_resource_id = azurerm_storage_account.storage.id
    subresource_names              = ["blob"]
  }
}
