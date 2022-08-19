
variable "network_prefix" {
  description = "Network prefix for Dev enviroment"
}

variable "location" {
  description = "location"
}

variable "env" {
  description = "Used to describe current enviroment, and to concatenate it to resource group"
}

variable "resource_group" {
  description = "Resource group name defined in locals."
}

variable "shared_peer_network_id" {
  description = "Shared peer network from another resource group."
}

variable "shared_rg_name" {
  description = "shared resource_group_name"
}

variable "shared_vnet_network_name" {
  description = "Network name of shared vnet vpn-hub"
}
