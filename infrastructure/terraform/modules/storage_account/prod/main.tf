#st - storage, dnsz - dns zone

resource "azurerm_storage_account" "storage" {
  name                     = "stmosaico${var.env}"
  resource_group_name      = var.resource_group
  location                 = var.location
  account_tier             = "Standard"
  account_replication_type = "GRS"
  min_tls_version           = "TLS1_2"
  access_tier               = "Hot"
  account_kind              = "StorageV2"
  allow_blob_public_access  = true
  # allow_nested_items_to_be_public = true - this is preparation for azurerm 3.x
  static_website {
    index_document          = "index.html"
    error_404_document      = "index.html"
  }
  lifecycle {
    ignore_changes = [
      custom_domain
    ]
  }
  custom_domain {
    name = "app.mosaico.ai"
    use_subdomain = true
  }
}

resource "azurerm_storage_management_policy" "example" {
  storage_account_id = azurerm_storage_account.storage.id

  rule {
    name    = "move-to-cold-7-days"
    enabled = true
    filters {
      prefix_match = ["backups/CORE_2","backups/IDENTITY_2"]
      blob_types   = ["blockBlob"]
    }
    actions {
      base_blob {
        tier_to_cool_after_days_since_modification_greater_than    = 7
        delete_after_days_since_modification_greater_than          = 40 # it has to be above 30+7 so why not 40 (above 30 because of fees when you use it earlier)
      }
    }
  }
}

#---------CUSTOM ROLE DEFINITION FOR SERVICE BUS------------------------------------------
resource "azurerm_role_definition" "mosaico_terraformshared_rule" {
  name        = "staccount_terraform_rule-${var.env}"
  scope       = "/subscriptions/${var.shared3_subscription_id}/resourceGroups/${var.shared3_resource_group}"
  description = "Role to access storage account from Pay as you go 3 Subscription"

  permissions {
    actions     = ["Microsoft.Storage/storageAccounts/listKeys/action"] #we need that action to properly export database as a backup
  }
  #assigning scope where service bus itself residses, and where our shared vnet is placed.
  assignable_scopes = [
    "/subscriptions/${var.shared3_subscription_id}/resourceGroups/${var.shared3_resource_group}"
  ]
}


resource "azurerm_private_dns_zone" "dns_zone" {
  name                = "privatelink.blob.core.windows.net"
  resource_group_name = var.resource_group
}

resource "azurerm_private_dns_zone_virtual_network_link" "dns_zone_vn_link" {
  name                  = "privatelink.blob.core.windows.net" #private dns zone virtual network link
  resource_group_name   = var.resource_group
  private_dns_zone_name = azurerm_private_dns_zone.dns_zone.name
  virtual_network_id    = var.vnet_main_id
}

resource "azurerm_private_endpoint" "private_endpoint" {
  name                = "pe-mosaico-st${var.env}" #private endpoint
  location            = var.location
  resource_group_name = var.resource_group
  subnet_id           = var.vnet_subnet_id

  private_dns_zone_group {
    name                 = "private-dns-zone-group"
    private_dns_zone_ids = [azurerm_private_dns_zone.dns_zone.id]
  }

  private_service_connection {
    name                           = "psc-st-mosaico${var.env}"
    is_manual_connection           = false
    private_connection_resource_id = azurerm_storage_account.storage.id
    subresource_names              = ["blob"]
  }
}
