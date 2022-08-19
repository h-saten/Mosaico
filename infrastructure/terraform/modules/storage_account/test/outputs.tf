output "primary_connection_string" {
  description = "Primary connection string"
  value       = azurerm_storage_account.storage.primary_connection_string
}

output "primary_endpoint" {
  description = "Primary endpoint string"
  value       = azurerm_storage_account.storage.primary_blob_endpoint
}

output "primary_web_host" {
  description = "Primary connection string"
  value       = azurerm_storage_account.storage.primary_web_host
}

output "dns_zone_id" {
  description = "output of private dns zone id"
  value = azurerm_private_dns_zone.dns_zone.id
}