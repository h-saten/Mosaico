resource "azurerm_redis_cache" "redis" {
  name                          = "redis-mosaico-${var.env}"
  location                      = var.location
  resource_group_name           = var.resource_group
  capacity                      = 1
  family                        = "P"
  sku_name                      = "Premium"
  enable_non_ssl_port           = false
  minimum_tls_version           = "1.2"
  public_network_access_enabled = false
  redis_version                 = 6
  subnet_id                     = var.subnet_id

  redis_configuration { #backup for redis, using storage account prod
    # rdb_backup_enabled            = true
    # rdb_backup_frequency          = 30
    # rdb_backup_max_snapshot_count = 5
    # rdb_storage_connection_string = var.storage_connection_string
  }
}