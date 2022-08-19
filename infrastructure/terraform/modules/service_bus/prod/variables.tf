variable "location" {
  description = "Location"
}

variable "env" {
  description = "Used to describe current enviroment, and to concatenate it to resource group"
}

variable "resource_group" {
  description = "Resource group name"
}

variable "vnet_subnet_id" {
  description = "Subnet for storage account private endpoint"
}

variable "topics" {
  description = "Topics for naming."
}

variable "shared_vnet_subnet_id" {
  description = "shared_vnet_subnet_id same as for peering"
}

variable "shared_rg_name" {
  description = "rg name from shared vnet"
}

variable "shared_vnet_network_name" {
  description = "name from shared vnet"
}

variable "shared_subscription_id" {
  description = "shared sub id from vnet shared"
}

variable "subscription_id" {
  description = "sub id of default sub"
}

variable "msi_id" {
  description = "sub id of default sub"
}

variable "vnet-mosaico-dev_snet_aks_virtual_nodes" {
  description = "aks virtual"
}

variable "vnet-mosaico-dev_snet_aks" {
  description = "snet aks"
}