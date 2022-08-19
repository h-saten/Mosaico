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

variable "shared3_subscription_id" {
  description = "Subscription Pay As You Go 3 ID"
}

variable "shared3_resource_group" {
  description = "Pay as You Go 3 resource group which holds storage holding terraform config"
}

variable "shared3_storage_account_name" {
  description = "Pay as You Go 3 storage account name "
}

variable "service_principal_id" {
  description = "Own Service Principal ID"
}