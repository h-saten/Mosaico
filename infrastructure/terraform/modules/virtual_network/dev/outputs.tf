output "vnet_main_id" {
  description = "Virtual network ID"
  value       = azurerm_virtual_network.vnet.id
}

output "vnet_main_name" {
  description = "Virtual network ID"
  value       = azurerm_virtual_network.vnet.name
}

output "default_subnet_id" {
  description = "ID of the default subnet in virtual network"
  value       = azurerm_subnet.default.id
}

output "gateway_subnet_id" {
  description = "ID of the private endpoints subnet in virtual network"
  value       = azurerm_subnet.gateway.id
}

output "aks_subnet_id" {
  description = "ID of the private endpoints subnet in virtual network"
  value       = azurerm_subnet.aks.id
}

output "aks_subnet_name" {
  description = "ID of the private endpoints subnet in virtual network"
  value       = azurerm_subnet.aks.name
}

output "aks_virtual_nodes_subnet_id" {
  description = "ID of the private endpoints subnet in virtual network"
  value       = azurerm_subnet.aks-virtual-nodes.id
}

output "shared_vnet_subnet_id" {
  description = "ID of shared vnet subnet"
  value       = data.azurerm_subnet.shared_subnet.id #using data source to output shared subnet id for sql firewall virtual network rule
}

output "shared_gateway_vnet_subnet_id" {
  description = "ID of shared vnet subnet"
  value       = data.azurerm_subnet.shared_gateway_subnet.id #using data source to output shared subnet id for sql firewall virtual network rule
}

output "snet_gateway_id" {
  description = "ID of snet gateway"
  value       = azurerm_subnet.gateway.id
}
