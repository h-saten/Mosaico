data "azurerm_resource_group" "mosaico_rg" {
  name = "rg-mosaico-${var.env}"
}

resource "azurerm_role_assignment" "storage_account_role_assignment" {
  scope                = "/subscriptions/${var.subscription_id}/resourceGroups/${local.resource_group}"
  role_definition_name = "Owner"
  principal_id         = var.dev_team_id
}
