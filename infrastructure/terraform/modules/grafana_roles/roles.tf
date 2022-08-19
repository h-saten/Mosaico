resource "azurerm_role_assignment" "Monitoring_Reader_RG" {
  principal_id         = var.service_principal_id           # Object ID
  role_definition_name = "Monitoring Reader" #scope cannot be only on the subnet, it has to be on main network
  scope                = "/subscriptions/${var.subscription_id}/resourceGroups/${var.resource_group}"
}