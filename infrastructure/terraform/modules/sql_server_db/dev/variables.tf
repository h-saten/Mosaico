variable "location" {
  description = "Location"
}

variable "env" {
  description = "Used to describe current enviroment, and to concatenate it to resource group"
}

variable "resource_group" {
  description = "Resource group name defined in locals."
}

variable "vnet_main_id" {
  description = "Subnet for storage account private endpoint"
}

variable "network_prefix" {
  description = "Network_prefix"
}

variable "shared_vnet_subnet_id" {
  description = "Network name of shared vnet vpn-hub"
}

variable "default_subnet_id" {
  description = "Default subnet ID"
}

variable "dev_team_id" {
  description = "ID of dev team."
}

variable "sqlserver_password" {
  description = "password of sql server"
}

variable "subscription_id" {
  description = "sub id"
}

variable "service_principal_id" {
  description = "sub id"
}