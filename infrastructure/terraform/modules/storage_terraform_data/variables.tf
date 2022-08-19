variable "location" {
  description = "location"
}

variable "env" {
  description = "Used to describe current enviroment, and to concatenate it to resource group"
}

variable "resource_group" {
  description = "Resource group name defined in locals."
}

variable "vnet_subnet_id" {
  description = "Subnet for storage account private endpoint"
}

variable "vnet_main_id" {
  description = "Subnet for storage account private endpoint"
}

variable "network_prefix" {
  description = "Network_prefix"
}

variable "service_principal_id" {
  description = "Own Service Principal ID"
}

variable "subscription_id" {
  description = "Subscription ID"
}

variable "dns_zone_id" {
  description = "Private dns zone id from main Storage Account"
}