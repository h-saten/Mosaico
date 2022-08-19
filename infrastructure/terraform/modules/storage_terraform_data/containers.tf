resource "azurerm_storage_container" "terraform_data_dev" {
  name                  = "v2-mosaico-dev"
  storage_account_name  = azurerm_storage_account.storage.name
  container_access_type = "private"
  lifecycle {
    ignore_changes = all
    prevent_destroy = true

  }

}

resource "azurerm_storage_container" "terraform_data_test" {
  name                  = "v2-mosaico-test"
  storage_account_name  = azurerm_storage_account.storage.name
  container_access_type = "private"
  lifecycle {
    ignore_changes = all
    prevent_destroy = true

  }
}

resource "azurerm_storage_container" "terraform_data_prod" {
  name                  = "v2-mosaico-prod"
  storage_account_name  = azurerm_storage_account.storage.name
  container_access_type = "private"
  lifecycle {
    prevent_destroy = true
    ignore_changes = all
  }
}

resource "azurerm_storage_container" "terraform_sapiency_state" {
  name                  = "sapiency"
  storage_account_name  = azurerm_storage_account.storage.name
  container_access_type = "private"
  lifecycle {
    prevent_destroy = true
    ignore_changes = all
  }
}
