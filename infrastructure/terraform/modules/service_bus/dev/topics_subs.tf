resource "azurerm_servicebus_topic" "topic_users" {
  for_each            = var.topics
  name                = each.key
  resource_group_name = var.resource_group
  namespace_name      = azurerm_servicebus_namespace.servicebus.name
}

#=============subscription======================
resource "azurerm_servicebus_subscription" "sb_subscription" {
  depends_on = [
    azurerm_servicebus_topic.topic_users
  ]
  for_each            = var.topics
  name                = "api"
  resource_group_name = var.resource_group
  namespace_name      = azurerm_servicebus_namespace.servicebus.name
  topic_name          = each.key
  max_delivery_count  = 1
}

#=============subscription id======================
resource "azurerm_servicebus_subscription" "sb_id_subscription" {
  depends_on = [
    azurerm_servicebus_topic.topic_users
  ]
  for_each            = var.topics
  name                = "id"
  resource_group_name = var.resource_group
  namespace_name      = azurerm_servicebus_namespace.servicebus.name
  topic_name          = each.key
  max_delivery_count  = 1
}