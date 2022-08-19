terraform {
  required_version = "~> 1.0.0"
  required_providers {
    googleworkspace = {
      source = "hashicorp/googleworkspace"
      version = "0.4.0"
    }
  }
}

provider "googleworkspace" {
  credentials             = var.credentials
  customer_id             = var.customer_id
  impersonated_user_email = var.impersonated_user_email
}

