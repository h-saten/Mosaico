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