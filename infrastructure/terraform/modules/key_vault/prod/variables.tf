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
  description = "ID of sp."
}

variable "dev_team_id" {
  description = "ID of dev team."
}

variable "st_primary_connection_string" {
  description = "Storage primary connection string"
}

variable "st_primary_endpoint" {
  description = "Storage primary endpoint"
}

variable "sb_primary_connection_string" {
  description = "Service Bus primary connection string"
}

variable "sql_server_fqdn" {
  description = ""
}

variable "sql_database_name" {
  description = ""
}

variable "sql_admin_Login" {
  description = ""
}

variable "sql_server_password" {
  description = ""
}

variable "sql_identity_database_name" {
  description = "Redis primary connection string"
}

variable "redis_primary_connection_string" {
  description = "redis conn str"
}

variable "msi_id" {
  description = "Managed identity id"
}

variable "instrumentation_key" {
  description = "Managed identity id"
}

variable "vnet_vpnhub_snet_default_id" {
  description = "Managed identity id"
}

variable "vnet-vpnhub-shared_default" {
  description = "vpnhub shared default net"
}

variable "vnet-mosaico-dev_snet_default" {
  description = "snet default"
}

variable "vnet-vpnhub-shared_gateway_subnet" {
  description = "gateway snet shared"
}

variable "vnet-mosaico-dev_snet_aks_virtual_nodes" {
  description = "aks virtual"
}

variable "vnet-mosaico-dev_snet_aks" {
  description = "snet aks"
}
