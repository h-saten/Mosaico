resource "azurerm_storage_container" "documents" {
  name                  = "ci-documents"
  storage_account_name  = azurerm_storage_account.storage.name
  container_access_type = "blob"
}

resource "azurerm_storage_container" "ntfs" {
  name                  = "ci-nfts"
  storage_account_name  = azurerm_storage_account.storage.name
  container_access_type = "container"
}

resource "azurerm_storage_container" "airdrop" {
  name                  = "ci-nft-metadata"
  storage_account_name  = azurerm_storage_account.storage.name
  container_access_type = "container"
}

resource "azurerm_storage_container" "terraform_data" {
  name                  = "ci-terraform-data"
  storage_account_name  = azurerm_storage_account.storage.name
  container_access_type = "private"
}

resource "azurerm_storage_container" "ci-project-team" {
  name                  = "ci-project-team-member-profile"
  storage_account_name  = azurerm_storage_account.storage.name
  container_access_type = "container"
}

resource "azurerm_storage_container" "ci-company-doc" {
  name                  = "ci-company-document-file"
  storage_account_name  = azurerm_storage_account.storage.name
  container_access_type = "private"
}

resource "azurerm_storage_container" "DollarWeb" {
  name                  = "$web"
  storage_account_name  = azurerm_storage_account.storage.name
  container_access_type = "private"
}

resource "azurerm_storage_container" "Backups" {
  name                  = "backups"
  storage_account_name  = azurerm_storage_account.storage.name
  container_access_type = "private"
}

resource "azurerm_storage_container" "Backups-redis" {
  name                  = "backups-redis"
  storage_account_name  = azurerm_storage_account.storage.name
  container_access_type = "private"
}