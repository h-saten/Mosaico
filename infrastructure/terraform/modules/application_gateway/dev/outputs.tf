output "gateway_id" {
  description = "Redis connection string for secret"
  value       = azurerm_application_gateway.gateway.id
}
