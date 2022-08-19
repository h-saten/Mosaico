output "connection_string" {
  description = "Redis connection string for secret"
  value       = azurerm_kubernetes_cluster.k8s.node_resource_group
}

output "instrumentation_key" {
  value = azurerm_application_insights.insights.instrumentation_key
}