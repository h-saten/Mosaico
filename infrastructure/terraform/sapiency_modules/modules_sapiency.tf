module "storage_account_sapiency_dev" {
  source                    = "./modules/storage_account_sapiency/dev"
#   count                     = var.env == "dev" ? 1 : 0
  resource_group            = "rg_sapiency_dev"
  env                       = var.env
  location                  = var.location
}

module "storage_account_sapiency_prod" {
  source                    = "./modules/storage_account_sapiency/prod"
#   count                     = var.env == "prod" ? 1 : 0
  resource_group            = "rg_sapiency_prod"
  env                       = var.env
  location                  = var.location
}

module "resource_groups_sapiency" {
  source                    = "./modules/resource_groups_sapiency"
  location                  = var.location
}