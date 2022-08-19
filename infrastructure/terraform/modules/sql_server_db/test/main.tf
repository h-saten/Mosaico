data "azurerm_client_config" "current" {}

locals {
  database_name          = "sqldb-${var.env}"
  identity_database_name = "sqldb-identity-${var.env}"
  admin_Login            = "mosaico"
}

resource "random_password" "sqlserver_password" {
  length           = 16
  special          = true
  override_special = "_%@"
}

resource "azurerm_mssql_server" "sql_server" {
  name                         = "sql-mosaico-${var.env}"
  resource_group_name          = var.resource_group
  location                     = var.location
  version                      = "12.0"
  administrator_login          = local.admin_Login
  administrator_login_password = random_password.sqlserver_password.result #randomized password
  minimum_tls_version          = "1.2"

  azuread_administrator {
    login_username = "mosaicoadmin"
    object_id      = var.dev_team_id
  }

  timeouts {
    create = "15m"
  }
}

resource "azurerm_sql_virtual_network_rule" "sql_server_net_rule" {

  name                = "sql-shared-hub-rule-${var.env}"
  resource_group_name = var.resource_group
  server_name         = azurerm_mssql_server.sql_server.name
  subnet_id           = var.shared_vnet_subnet_id
}

resource "azurerm_sql_virtual_network_rule" "sql_server_default_net_rule" {

  name                = "sql-vnet-default-rule-${var.env}"
  resource_group_name = var.resource_group
  server_name         = azurerm_mssql_server.sql_server.name
  subnet_id           = var.default_subnet_id
}

resource "azurerm_mssql_database" "sql_database" {
  name                 = local.database_name
  server_id            = azurerm_mssql_server.sql_server.id
  collation            = "SQL_Latin1_General_CP1_CI_AS"
  max_size_gb          = 100
  sku_name             = "S1"
  storage_account_type = "GRS"
  #I dont specify license_type here, because i think it created issues with deployment timeouts.

  timeouts {
    create = "15m"
  }
}

resource "azurerm_private_dns_zone" "dns_zone" {
  name                = "privatelink.database.windows.net"
  resource_group_name = var.resource_group
}

resource "azurerm_private_dns_zone_virtual_network_link" "dns_zone_vn_link" {
  name                  = "pl-sql-dns" #not sure if that link is necessary
  resource_group_name   = var.resource_group
  private_dns_zone_name = azurerm_private_dns_zone.dns_zone.name
  virtual_network_id    = var.vnet_main_id
}

resource "azurerm_private_endpoint" "private_endpoint" {

  name                = "pe-sql-mosaico-${var.env}"
  location            = var.location
  resource_group_name = var.resource_group
  subnet_id           = var.default_subnet_id

  private_dns_zone_group {
    name                 = "private-dns-zone-group"
    private_dns_zone_ids = [azurerm_private_dns_zone.dns_zone.id]
  }

  private_service_connection {
    name                           = "psc-kv-mosaico${var.env}"
    is_manual_connection           = false
    private_connection_resource_id = azurerm_mssql_server.sql_server.id
    subresource_names              = ["sqlServer"]
  }
}

resource "null_resource" "disable_public_access" {
  depends_on = [
    azurerm_sql_virtual_network_rule.sql_server_default_net_rule,
    azurerm_sql_virtual_network_rule.sql_server_net_rule
  ]

  provisioner "local-exec" {
    command = "az sql server update -g ${var.resource_group} -n sql-mosaico-${var.env} --set publicNetworkAccess='Disabled'"
  }
}