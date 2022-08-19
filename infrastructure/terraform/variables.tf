variable "subscription_id" {
  type = string

  # validation {
  #   condition     = length(var.subscription_id) == 36
  #   error_message = "Subscription_id has to be 36 chars."
  # }

  description = "Provides Subscription ID"
}

variable "client_id" {
  type = string

  # validation {
  #   condition     = length(var.client_id) == 36
  #   error_message = "Client_id has to be 36 chars."
  # }

  description = "Provides client_ID"
}

variable "client_secret" {
  type = string

  validation {
    condition     = length(var.client_secret) == 37 || length(var.client_secret) == 34
    error_message = "Client_secret has to be 37 or 34 chars."
  }

  description = "Provides secret key"
}

variable "tenant_id" {
  type = string

  validation {
    condition     = length(var.tenant_id) == 36
    error_message = "Tenant_id has to be 36 chars."
  }

  description = "Provides tenant_id"
}

variable "shared_subscription_id" {
  type = string

  # validation {
  #   condition     = length(var.shared_subscription_id) == 36
  #   error_message = "Subscription_id has to be 36 chars."
  # }

  description = "Provides Subscription ID"
}

variable "shared_client_id" {
  type = string

  # validation {
  #   condition     = length(var.shared_client_id) == 36
  #   error_message = "Client_id has to be 36 chars."
  # }

  description = "Provides client_ID"
}

variable "shared_client_secret" {
  type = string

  # validation {
  #   condition     = length(var.shared_client_secret) == 37
  #   error_message = "Client_secret has to be 37 chars."
  # }

  description = "Provides secret key"
}

variable "shared_tenant_id" {
  type = string

  # validation {
  #   condition     = length(var.shared_tenant_id) == 36
  #   error_message = "Tenant_id has to be 36 chars."
  # }

  description = "Provides tenant_id"
}

variable "shared3_subscription_id" {
  type = string

  # validation {
  #   condition     = length(var.shared3_subscription_id) == 36
  #   error_message = "Subscription_id has to be 36 chars."
  # }

  description = "Provides Subscription ID"
}

variable "shared3_client_id" {
  type = string

  # validation {
  #   condition     = length(var.shared3_client_id) == 36
  #   error_message = "Client_id has to be 36 chars."
  # }

  description = "Provides client_ID"
}

variable "shared3_client_secret" {
  type = string

  # validation {
  #   condition     = length(var.shared3_client_secret) == 37
  #   error_message = "Client_secret has to be 37 chars."
  # }

  description = "Provides secret key"
}

variable "shared3_tenant_id" {
  type = string

  # validation {
  #   condition     = length(var.shared3_tenant_id) == 36
  #   error_message = "Tenant_id has to be 36 chars."
  # }

  description = "Provides tenant_id"
}

variable "shared3_rg_name" {
  type        = string
  description = "Provides resource group name for subscription pay as you go3"
}

variable "shared3_acr_name" {
  type        = string
  description = "Provides acr name from subscription pay as you go 3 to use in aks"
}

variable "shared3_st_acc_name" {
  type        = string
  description = "Provides storage account name from subscription pay as you go 3 to use in role assignment on storage_account module"
}

variable "env" {
  type = string

  validation {
    condition     = can(regex("^(prod|dev|test)$", var.env))
    error_message = "The env must be set to: dev,test or prod."
  }

  description = "Specify enviroment you want to deploy in: dev/test/prod"
}

variable "network_prefix" {
  type = string

  validation {
    condition     = can(regex("^(17|18|19)$", var.network_prefix))
    error_message = "The network prefix has to be set to: 17/18/19."
  }

  description = "describes network prefix to use on specific enviroment."
}

variable "location" {
  type = string

  validation {
    condition     = can(regex("^(West Europe|North Europe)$", var.location))
    error_message = "The Location has to be set as West/North Europe."
  }

  default = "West Europe"

  description = "Describe location."
}

variable "service_principal_id" {
  type = string

  validation {
    condition     = length(var.service_principal_id) == 36
    error_message = "Service_principal_id has to be 36 chars."
  }

  description = "Provides service principal id."
}

variable "dev_team_id" {
  type = string

  validation {
    condition     = length(var.dev_team_id) == 36
    error_message = "Dev_team_id has to be 36 chars."
  }

  description = "Provides dev team id."
}

variable "shared_rg_name" {
  type = string

  description = "Provides shared resource group name"
}

variable "shared_vnet_network_name" {
  type = string

  description = "Provides shared vnet name."
}

variable "client_certificate_path" {
  type = string

  description = "Provides shared vnet name."
}

variable "client_certificate_password" {
  type = string
  
  description = "Provides shared vnet name."
}

variable "sqlserver_password" {
  type = string
  
  description = "Password for sqlserver"
}
