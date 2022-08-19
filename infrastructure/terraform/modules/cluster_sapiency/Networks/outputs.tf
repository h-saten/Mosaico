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

output "aks_subnet_id" {
  description = "ID of the private endpoints subnet in virtual network"
  value       = azurerm_subnet.aks.id
}

output "aks_subnet_name" {
  description = "ID of the private endpoints subnet in virtual network"
  value       = azurerm_subnet.aks.name
}