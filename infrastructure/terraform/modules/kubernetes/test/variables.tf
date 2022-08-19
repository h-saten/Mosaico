variable "location" {
  description = "Location"
}

variable "env" {
  description = "Used to describe current enviroment, and to concatenate it to resource group"
}

variable "resource_group" {
  description = "Resource group name defined in locals."
}

variable "tenant_id" {
  description = "Tenant ID value provided from ROOT (env.tfvars)"
}

variable "dev_team_id" {
  description = "ID of dev team."
}

variable "client_secret" {
  description = "Client secret"
}

variable "client_id" {
  description = "Client ID"
}

variable "aks_subnet_id" {
  description = "Virtual network for node pool"
}

variable "managed_identity_id" {
  description = "ID of managed identity"
}

variable "subscription_id" {
  description = "Subscription ID"
}

variable "virtual_network" {
  description = "Main virtual network"
}

variable "msi_id" {
  description = "Managed identity id"
}

variable "acr_id" {
  description = "ID of our container registry"
}
