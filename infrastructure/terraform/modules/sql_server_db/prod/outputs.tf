output "sql_server_fqdn" {
  description = "SQL Server address"
  value       = azurerm_mssql_server.sql_server.fully_qualified_domain_name
}

output "sql_database_name" {
  description = "SQL database name"
  value       = local.database_name
}

output "sql_identity_database_name" {
  description = "SQL database name"
  value       = local.identity_database_name
}

output "sql_admin_Login" {
  description = "Admin user login"
  value       = local.admin_Login
}

output "sql_server_password" {
  description = "Sql server password"
  value       = var.sqlserver_password
}