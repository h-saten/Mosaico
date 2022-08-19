terraform {

  # backend "azurerm" {
  #   resource_group_name  = "rg-mosaico-shared"
  #   storage_account_name = "stmosaicoterraformshared"
  #   container_name       = "v2-mosaico-dev"    #change manually to dev/test/prod if you want to deploy to said enviroment.      
  #   key                  = "terraform.tfstate" #when changing enviroment remember to init again with -reconfigure
  #   subscription_id      = "336a119c-abfd-452a-9bca-ae83fb45c62b"
  # }

  backend "azurerm" {
    resource_group_name  = "rg-mosaico-terraform"
    storage_account_name = "stmosaicoterraform"
    container_name       = "v2-mosaico-prod"    #change manually to dev/test/prod if you want to deploy to said enviroment.      
    key                  = "terraform.tfstate" #when changing enviroment remember to init again with -reconfigure
    subscription_id      = "6eb5c473-142c-4043-a506-cc94fdbedce0"
  }

  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "2.88.1" #update for new version, whenever a new one is available manually, latest: 2.88.1
    }
    azuread = {
      source  = "hashicorp/azuread"
      version = "2.12.0"
    }
    cloudflare = {
      source = "cloudflare/cloudflare"
      version = ">= 3.3.0"
    }
  }
}

provider "azurerm" {
  features {}
  skip_provider_registration = true
  subscription_id            = var.subscription_id
  tenant_id                  = var.tenant_id
  client_id                  = var.client_id
  client_secret              = var.client_secret
  #client_certificate_path     = var.client_certificate_path
  #client_certificate_password = var.client_certificate_password
}

provider "azurerm" {
  features {}
  alias                      = "shared"
  skip_provider_registration = true
  subscription_id            = var.shared_subscription_id
  tenant_id                  = var.shared_tenant_id
  client_id                  = var.shared_client_id
  client_secret              = var.shared_client_secret
  #client_certificate_path     = var.client_certificate_path
  #client_certificate_password = var.client_certificate_password
}

provider "azurerm" {
  features {}
  alias                      = "shared3"      #better name
  skip_provider_registration = true
  subscription_id            = var.shared3_subscription_id
  tenant_id                  = var.shared3_tenant_id
  client_id                  = var.shared3_client_id
  client_secret              = var.shared3_client_secret
  #client_certificate_path     = var.client_certificate_path
  #client_certificate_password = var.client_certificate_password
}


provider "azuread" {
  tenant_id = var.tenant_id
}

provider "cloudflare" {
  email   = var.cloudflare_email
  api_key = var.cloudflare_api_key
}

locals {
  # resource_group = "rg-mosaico-prod"
  resource_group = "rg-mosaico-${var.env}"
  resource_group_terraform = "rg-mosaico-terraform"
  topics         = toset(["users", "wallets", "projects", "nfts", "documents", "companies","features"]) #topics set used by service bus
}