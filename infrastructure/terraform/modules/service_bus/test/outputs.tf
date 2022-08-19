output "primary_connection_string" {
  description = "Primary connection string"
  value       = azurerm_servicebus_namespace_authorization_rule.shared_acces_policy.primary_connection_string
}