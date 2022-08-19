variable "location" {
  description = "Location"
}

variable "env" {
  description = "Used to describe current enviroment, and to concatenate it to resource group"
}

variable "resource_group" {
  description = "Resource group name defined in locals."
}

variable "virtual_network" {
  description = "Subnet for storage account private endpoint"
}

variable "key_vault_secret_id" {
  description = ""
}

variable "subnet_gateway_id" {
  description = ""
}

variable "sku_type" {
  description = ""
}

variable "agw_identity_id" {
  description = ""
}

variable "subscription_id" {
  description = "Subscription ID"
}

variable "key_vault_name" {
  description = "Subscription ID"
}

variable "agw_identity_principal_id" {
  description = "Managed identity id"
}
