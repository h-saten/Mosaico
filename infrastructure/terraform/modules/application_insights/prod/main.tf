data "azurerm_log_analytics_workspace" "log_analytics_workspace" {
  name                = "log-mosaico-${var.env}"
  resource_group_name = var.resource_group
}

resource "azurerm_application_insights" "insights" {
  name                = "appi-relay-${var.env}"
  location            = var.location
  resource_group_name = var.resource_group
  application_type    = "Node.JS"
  workspace_id        = data.azurerm_log_analytics_workspace.log_analytics_workspace.id
  daily_data_cap_in_gb = 1
  daily_data_cap_notifications_disabled = false
  retention_in_days = 30
}