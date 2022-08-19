resource "azurerm_servicebus_namespace" "servicebus" {
  name                = "sbmosaico${var.env}"
  location            = var.location
  resource_group_name = var.resource_group
  sku                 = "Standard"
  capacity            = 0
}

resource "azurerm_servicebus_namespace_authorization_rule" "shared_acces_policy" {
  name                = "ApiSharedAccessPolicy"
  namespace_name      = azurerm_servicebus_namespace.servicebus.name
  resource_group_name = var.resource_group

  listen = true
  send   = true
  manage = true
}

resource "azurerm_role_definition" "service_bus_shared" {
  name        = "vpnhub-validate-action"
  scope       = "/subscriptions/${var.subscription_id}/resourceGroups/${var.resource_group}/providers/Microsoft.ServiceBus/namespaces/${azurerm_servicebus_namespace.servicebus.name}"
  description = "This is a custom role created via Terraform"

  permissions {
    actions     = ["Microsoft.Network/virtualNetworks/taggedTrafficConsumers/validate/action"] #we need that action to properly assign virtual network shared to service bus
  }
  #assigning scope where service bus itself residses, and where our shared vnet is placed.
  assignable_scopes = [
    "/subscriptions/${var.subscription_id}/resourceGroups/${var.resource_group}/providers/Microsoft.ServiceBus/namespaces/${azurerm_servicebus_namespace.servicebus.name}","/subscriptions/${var.shared_subscription_id}/resourceGroups/${var.shared_rg_name}/providers/Microsoft.Network/virtualNetworks/${var.shared_vnet_network_name}/","/subscriptions/${var.shared_subscription_id}/resourceGroups/${var.shared_rg_name}/providers/Microsoft.Network/virtualNetworks/${var.shared_vnet_network_name}/taggedTrafficConsumers/Microsoft.ServiceBus"
  ]
}

# resource "azurerm_role_assignment" "example" {
#   scope              = "/subscriptions/115e50a3-131b-454b-b168-1aedb4ade9c3/resourceGroups/rg-vpnhub-shared/providers/Microsoft.Network/virtualNetworks/vnet-vpnhub-shared/taggedTrafficConsumers/Microsoft.ServiceBus"
#   role_definition_id = azurerm_role_definition.service_bus_shared.role_definition_resource_id
#   principal_id       = "3a3f6d1d-d042-4cd4-953f-77a4506858ca"
# }

resource "azurerm_role_assignment" "example" {
  scope              = "/subscriptions/${var.shared_subscription_id}/resourceGroups/${var.shared_rg_name}/providers/Microsoft.Network/virtualNetworks/${var.shared_vnet_network_name}/"
  role_definition_id = azurerm_role_definition.service_bus_shared.role_definition_resource_id
  principal_id       = var.msi_id
}

resource "azurerm_servicebus_namespace_network_rule_set" "servicebus" {
  depends_on = [
    azurerm_servicebus_namespace.servicebus
  ]
  namespace_name      = azurerm_servicebus_namespace.servicebus.name
  resource_group_name = var.resource_group
  default_action      = "Deny"
  trusted_services_allowed = true

  network_rules {
    subnet_id                            = var.vnet_subnet_id
    ignore_missing_vnet_service_endpoint = false
  }
  network_rules {
    subnet_id                            = var.shared_vnet_subnet_id
    ignore_missing_vnet_service_endpoint = false
  }
  network_rules {
    subnet_id                            = var.vnet-mosaico-dev_snet_aks_virtual_nodes
    ignore_missing_vnet_service_endpoint = false
  }
  network_rules {
    subnet_id                            = var.vnet-mosaico-dev_snet_aks
    ignore_missing_vnet_service_endpoint = false
  }
}


resource "azurerm_private_dns_zone" "servicebus" {
  depends_on = [
    azurerm_servicebus_namespace.servicebus
  ]
  name                = "privatelink.servicebus.windows.net"
  resource_group_name = var.resource_group
}

resource "azurerm_private_endpoint" "servicebus" {
  depends_on = [
    azurerm_private_dns_zone.servicebus
  ]
  name                = "pe-sb-mosaico-${var.env}"
  location            = var.location
  resource_group_name = var.resource_group
  subnet_id           = var.vnet_subnet_id

  private_service_connection {
    name                           = "psc-sb-mosaico${var.env}"
    private_connection_resource_id = azurerm_servicebus_namespace.servicebus.id
    is_manual_connection           = false
    subresource_names              = ["namespace"]
  }

  private_dns_zone_group {
    name                 = "pscsbmosaico${var.env}"
    private_dns_zone_ids = [azurerm_private_dns_zone.servicebus.id]
  }
}