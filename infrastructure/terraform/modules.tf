module "virtual_network_dev" {
  count                    = var.env == "dev" ? 1 : 0
  source                   = "./modules/virtual_network/dev"
  network_prefix           = var.network_prefix
  location                 = var.location
  env                      = var.env
  resource_group           = local.resource_group
  shared_peer_network_id   = data.azurerm_virtual_network.vn_shared.id
  shared_rg_name           = var.shared_rg_name
  shared_vnet_network_name = var.shared_vnet_network_name

  providers = {
    azurerm                = azurerm        #use default provider
    azurerm.shared         = azurerm.shared #use shared provider - another subscription, to utilize its vnet for peering.
  }
}

module "virtual_network_test" {
  count                    = var.env == "test" ? 1 : 0
  source                   = "./modules/virtual_network/test"
  network_prefix           = var.network_prefix
  location                 = var.location
  env                      = var.env
  resource_group           = local.resource_group
  shared_peer_network_id   = data.azurerm_virtual_network.vn_shared.id
  shared_rg_name           = var.shared_rg_name
  shared_vnet_network_name = var.shared_vnet_network_name

  providers = {
    azurerm                = azurerm        #use default provider
    azurerm.shared         = azurerm.shared #use shared provider - another subscription, to utilize its vnet for peering.
  }
}

module "virtual_network_prod" {
  count                    = var.env == "prod" ? 1 : 0 #we create 0 of specific module, if env variable doesn't match provided enviroment.
  source                   = "./modules/virtual_network/prod"
  network_prefix           = var.network_prefix
  location                 = var.location
  env                      = var.env
  resource_group           = local.resource_group
  shared_peer_network_id   = data.azurerm_virtual_network.vn_shared.id
  shared_rg_name           = var.shared_rg_name
  shared_vnet_network_name = var.shared_vnet_network_name

  providers = {
    azurerm                 = azurerm        #use default provider
    azurerm.shared          = azurerm.shared #use shared provider - another subscription, to utilize its vnet for peering.
  }
}

module "key_vault_dev" {
  depends_on = [
    module.virtual_network_dev
  ]
  count                                   = var.env == "dev" ? 1 : 0
  source                                  = "./modules/key_vault/dev"
  tenant_id                               = var.tenant_id
  network_prefix                          = var.network_prefix
  location                                = var.location
  env                                     = var.env
  vnet_subnet_id                          = module.virtual_network_dev[0].default_subnet_id
  vnet_main_id                            = module.virtual_network_dev[0].vnet_main_id
  resource_group                          = local.resource_group
  service_principal_id                    = var.service_principal_id
  dev_team_id                             = var.dev_team_id
  msi_id                                  = azurerm_user_assigned_identity.agw_identity.principal_id
  vnet_vpnhub_snet_default_id             = module.virtual_network_dev[0].shared_vnet_subnet_id
  #---------------------------FIREWALL VNETS------------------------------------------------------------
  vnet-vpnhub-shared_default              = module.virtual_network_dev[0].shared_vnet_subnet_id
  vnet-mosaico-dev_snet_default           = module.virtual_network_dev[0].default_subnet_id
  vnet-vpnhub-shared_gateway_subnet       = module.virtual_network_dev[0].shared_gateway_vnet_subnet_id
  vnet-mosaico-dev_snet_aks_virtual_nodes = module.virtual_network_dev[0].aks_virtual_nodes_subnet_id
  vnet-mosaico-dev_snet_aks               = module.virtual_network_dev[0].aks_subnet_id
  #---------------------STORAGE ACCOUNT SECRETS---------------------------------------------------------
  st_primary_connection_string            = module.storage_account_dev[0].primary_connection_string
  st_primary_endpoint                     = module.storage_account_dev[0].primary_endpoint
  #----------------------SERVICE BUS SECRETS------------------------------------------------------------
  sb_primary_connection_string            = module.service_bus_dev[0].primary_connection_string
  #--------------------------SQL SECRETS----------------------------------------------------------------
  sql_admin_Login                         = module.sql_server_dev[0].sql_admin_Login
  sql_database_name                       = module.sql_server_dev[0].sql_database_name
  sql_server_fqdn                         = module.sql_server_dev[0].sql_server_fqdn
  sql_server_password                     = var.sqlserver_password
   #--------------------------SQL IDENTITY SECRETS-------------------------------------------------------
  sql_identity_database_name              = module.sql_server_dev[0].sql_identity_database_name
  #----------------------------REDIS--------------------------------------------------------------------
  redis_primary_connection_string         = ""
  #--------------------------APP INSIGHTS INSTRUMENTATION KEY-------------------------------------------
  instrumentation_key                     = module.aks_dev[0].instrumentation_key
}

module "key_vault_test" {
  depends_on = [
    module.virtual_network_test
  ]
  count                                   = var.env == "test" ? 1 : 0
  source                                  = "./modules/key_vault/test"
  tenant_id                               = var.tenant_id
  network_prefix                          = var.network_prefix
  location                                = var.location
  env                                     = var.env
  vnet_subnet_id                          = module.virtual_network_test[0].default_subnet_id
  vnet_main_id                            = module.virtual_network_test[0].vnet_main_id
  resource_group                          = local.resource_group
  service_principal_id                    = var.service_principal_id
  dev_team_id                             = var.dev_team_id
  msi_id                                  = azurerm_user_assigned_identity.agw_identity.principal_id
  vnet_vpnhub_snet_default_id             = module.virtual_network_test[0].shared_vnet_subnet_id
  #---------------------------FIREWALL VNETS------------------------------------------------------------
  vnet-vpnhub-shared_default              = module.virtual_network_test[0].shared_vnet_subnet_id
  vnet-mosaico-test_snet_default           = module.virtual_network_test[0].default_subnet_id
  vnet-vpnhub-shared_gateway_subnet       = module.virtual_network_test[0].shared_gateway_vnet_subnet_id
  vnet-mosaico-test_snet_aks_virtual_nodes = module.virtual_network_test[0].aks_virtual_nodes_subnet_id
  vnet-mosaico-test_snet_aks               = module.virtual_network_test[0].aks_subnet_id
  #---------------------STORAGE ACCOUNT SECRETS---------------------------------------------------------
  st_primary_connection_string            = module.storage_account_test[0].primary_connection_string
  st_primary_endpoint                     = module.storage_account_test[0].primary_endpoint
  #----------------------SERVICE BUS SECRETS------------------------------------------------------------
  sb_primary_connection_string            = module.service_bus_test[0].primary_connection_string
  #--------------------------SQL SECRETS----------------------------------------------------------------
  sql_admin_Login                         = module.sql_server_test[0].sql_admin_Login
  sql_database_name                       = module.sql_server_test[0].sql_database_name
  sql_server_fqdn                         = module.sql_server_test[0].sql_server_fqdn
  sql_server_password                     = var.sqlserver_password
   #--------------------------SQL IDENTITY SECRETS-------------------------------------------------------
  sql_identity_database_name              = module.sql_server_test[0].sql_identity_database_name
  #----------------------------REDIS--------------------------------------------------------------------
  redis_primary_connection_string         = ""
  #--------------------------APP INSIGHTS INSTRUMENTATION KEY-------------------------------------------
  instrumentation_key                     = module.aks_test[0].instrumentation_key
}

module "key_vault_prod" {
  depends_on = [
    module.virtual_network_prod
  ]

  count                          = var.env == "prod" ? 1 : 0
  source                         = "./modules/key_vault/prod"
  tenant_id                      = var.tenant_id
  network_prefix                 = var.network_prefix
  location                       = var.location
  env                            = var.env
  vnet_subnet_id                 = module.virtual_network_prod[0].default_subnet_id
  vnet_main_id                   = module.virtual_network_prod[0].vnet_main_id
  resource_group                 = local.resource_group
  service_principal_id           = var.service_principal_id
  dev_team_id                    = var.dev_team_id
  msi_id                         = azurerm_user_assigned_identity.agw_identity.principal_id
  vnet_vpnhub_snet_default_id             = module.virtual_network_prod[0].shared_vnet_subnet_id
  #---------------------------FIREWALL VNETS------------------------------------------------------------
  vnet-vpnhub-shared_default              = module.virtual_network_prod[0].shared_vnet_subnet_id
  vnet-mosaico-dev_snet_default           = module.virtual_network_prod[0].default_subnet_id
  vnet-vpnhub-shared_gateway_subnet       = module.virtual_network_prod[0].shared_gateway_vnet_subnet_id
  vnet-mosaico-dev_snet_aks_virtual_nodes = module.virtual_network_prod[0].aks_virtual_nodes_subnet_id
  vnet-mosaico-dev_snet_aks               = module.virtual_network_prod[0].aks_subnet_id
  #---------------------STORAGE ACCOUNT SECRETS---------------------------------------------------------
  st_primary_connection_string   = module.storage_account_prod[0].primary_connection_string
  st_primary_endpoint            = module.storage_account_prod[0].primary_endpoint
  #----------------------SERVICE BUS SECRETS------------------------------------------------------------
  sb_primary_connection_string   = module.service_bus_prod[0].primary_connection_string
  #--------------------------SQL SECRETS----------------------------------------------------------------
  sql_admin_Login                = module.sql_server_prod[0].sql_admin_Login
  sql_database_name              = module.sql_server_prod[0].sql_database_name
  sql_server_fqdn                = module.sql_server_prod[0].sql_server_fqdn
  sql_server_password            = module.sql_server_prod[0].sql_server_password
  #--------------------------SQL IDENTITY SECRETS-------------------------------------------------------
  sql_identity_database_name     = module.sql_server_prod[0].sql_identity_database_name
  #----------------------------REDIS--------------------------------------------------------------------
  redis_primary_connection_string = ""
  #--------------------------APP INSIGHTS INSTRUMENTATION KEY-------------------------------------------
  instrumentation_key             = module.aks_prod[0].instrumentation_key
}

module "storage_account_dev" {

  depends_on = [
    module.virtual_network_dev
  ]

  count                        = var.env == "dev" ? 1 : 0
  source                       = "./modules/storage_account/dev"
  location                     = var.location
  env                          = var.env
  network_prefix               = var.network_prefix
  vnet_subnet_id               = module.virtual_network_dev[0].aks_subnet_id
  vnet_main_id                 = module.virtual_network_dev[0].vnet_main_id
  resource_group               = local.resource_group
  shared3_subscription_id      = var.shared3_subscription_id
  shared3_resource_group       = var.shared3_rg_name
  shared3_storage_account_name = var.shared3_st_acc_name
  service_principal_id         = var.service_principal_id
}

module "storage_account_test" {
  depends_on = [
    module.virtual_network_test
  ]

  count                        = var.env == "test" ? 1 : 0
  source                       = "./modules/storage_account/test"
  location                     = var.location
  env                          = var.env
  network_prefix               = var.network_prefix
  vnet_subnet_id               = module.virtual_network_test[0].aks_subnet_id
  vnet_main_id                 = module.virtual_network_test[0].vnet_main_id
  resource_group               = local.resource_group
  shared3_subscription_id      = var.shared3_subscription_id
  shared3_resource_group       = var.shared3_rg_name
  shared3_storage_account_name = var.shared3_st_acc_name
  service_principal_id         = var.service_principal_id
}

module "storage_account_prod" {
  depends_on = [
    module.virtual_network_prod
  ]

  count                        = var.env == "prod" ? 1 : 0
  source                       = "./modules/storage_account/prod"
  location                     = var.location
  env                          = var.env
  network_prefix               = var.network_prefix
  vnet_subnet_id               = module.virtual_network_prod[0].aks_subnet_id
  vnet_main_id                 = module.virtual_network_prod[0].vnet_main_id
  resource_group               = local.resource_group
  shared3_subscription_id      = var.shared3_subscription_id
  shared3_resource_group       = var.shared3_rg_name
  shared3_storage_account_name = var.shared3_st_acc_name
  service_principal_id         = var.service_principal_id
}
#DISABLE ON DEPLOY TO TEST/PROD, [OUT OF STATE]
# module "storage_account_terraform" {
#   depends_on = [
#     module.virtual_network_dev
#   ]
#   count                 = var.env == "dev" ? 1 : 0
#   source                = "./modules/storage_terraform_data"
#   location              = var.location
#   env                   = var.env
#   network_prefix        = var.network_prefix
#   vnet_subnet_id        = module.virtual_network_dev[0].aks_subnet_id
#   vnet_main_id          = module.virtual_network_dev[0].vnet_main_id
#   resource_group        = local.resource_group_terraform
#   subscription_id       = var.subscription_id
#   service_principal_id  = var.service_principal_id
#   dns_zone_id           = module.storage_account_dev[0].dns_zone_id
# }

module "service_bus_dev" {
  count                                   = var.env == "dev" ? 1 : 0
  source                                  = "./modules/service_bus/dev"
  location                                = var.location
  env                                     = var.env
  resource_group                          = local.resource_group
  vnet_subnet_id                          = module.virtual_network_dev[0].default_subnet_id
  topics                                  = local.topics
  shared_vnet_subnet_id                   = module.virtual_network_dev[0].shared_vnet_subnet_id
  shared_subscription_id                  = var.shared_subscription_id
  shared_rg_name                          = var.shared_rg_name
  shared_vnet_network_name                = var.shared_vnet_network_name
  subscription_id                         = var.subscription_id
  msi_id                                  = azurerm_user_assigned_identity.sb_identity.principal_id
  vnet-mosaico-dev_snet_aks_virtual_nodes = module.virtual_network_dev[0].aks_virtual_nodes_subnet_id
  vnet-mosaico-dev_snet_aks               = module.virtual_network_dev[0].aks_subnet_id
}

module "service_bus_test" {
  count                                   = var.env == "test" ? 1 : 0
  source                                  = "./modules/service_bus/test"
  location                                = var.location
  env                                     = var.env
  resource_group                          = local.resource_group
  vnet_subnet_id                          = module.virtual_network_test[0].default_subnet_id
  topics                                  = local.topics
  shared_vnet_subnet_id                   = module.virtual_network_test[0].shared_vnet_subnet_id
  shared_subscription_id                  = var.shared_subscription_id
  shared_rg_name                          = var.shared_rg_name
  shared_vnet_network_name                = var.shared_vnet_network_name
  subscription_id                         = var.subscription_id
  msi_id                                  = azurerm_user_assigned_identity.sb_identity.principal_id
  vnet-mosaico-dev_snet_aks_virtual_nodes = module.virtual_network_test[0].aks_virtual_nodes_subnet_id
  vnet-mosaico-dev_snet_aks               = module.virtual_network_test[0].aks_subnet_id
}

module "service_bus_prod" {
  count                                   = var.env == "prod" ? 1 : 0
  source                                  = "./modules/service_bus/prod"
  location                                = var.location
  env                                     = var.env
  resource_group                          = local.resource_group
  vnet_subnet_id                          = module.virtual_network_prod[0].default_subnet_id
  topics                                  = local.topics
  shared_vnet_subnet_id                   = module.virtual_network_prod[0].shared_vnet_subnet_id
  shared_subscription_id                  = var.shared_subscription_id
  shared_rg_name                          = var.shared_rg_name
  shared_vnet_network_name                = var.shared_vnet_network_name
  subscription_id                         = var.subscription_id
  msi_id                                  = azurerm_user_assigned_identity.sb_identity.principal_id
  vnet-mosaico-dev_snet_aks_virtual_nodes = module.virtual_network_prod[0].aks_virtual_nodes_subnet_id
  vnet-mosaico-dev_snet_aks               = module.virtual_network_prod[0].aks_subnet_id
}

module "sql_server_dev" {
  depends_on = [
    module.virtual_network_dev
  ]
  count                 = var.env == "dev" ? 1 : 0
  source                = "./modules/sql_server_db/dev"
  network_prefix        = var.network_prefix
  resource_group        = local.resource_group
  location              = var.location
  env                   = var.env
  default_subnet_id     = module.virtual_network_dev[0].default_subnet_id
  vnet_main_id          = module.virtual_network_dev[0].vnet_main_id
  shared_vnet_subnet_id = module.virtual_network_dev[0].shared_vnet_subnet_id
  dev_team_id           = var.dev_team_id
  sqlserver_password    = var.sqlserver_password
  service_principal_id  = var.service_principal_id
  subscription_id       = var.subscription_id
}

module "sql_server_test" {
  depends_on = [
    module.virtual_network_dev
  ]
  count                 = var.env == "test" ? 1 : 0
  source                = "./modules/sql_server_db/test"
  network_prefix        = var.network_prefix
  resource_group        = local.resource_group
  location              = var.location
  env                   = var.env
  default_subnet_id     = module.virtual_network_test[0].default_subnet_id
  vnet_main_id          = module.virtual_network_test[0].vnet_main_id
  shared_vnet_subnet_id = module.virtual_network_test[0].shared_vnet_subnet_id
  dev_team_id           = var.dev_team_id
}

module "sql_server_prod" {
  depends_on = [
    module.virtual_network_dev
  ]
  count                 = var.env == "prod" ? 1 : 0
  source                = "./modules/sql_server_db/prod"
  network_prefix        = var.network_prefix
  resource_group        = local.resource_group
  location              = var.location
  env                   = var.env
  default_subnet_id     = module.virtual_network_prod[0].default_subnet_id
  vnet_main_id          = module.virtual_network_prod[0].vnet_main_id
  shared_vnet_subnet_id = module.virtual_network_prod[0].shared_vnet_subnet_id
  dev_team_id           = var.dev_team_id
  sqlserver_password    = var.sqlserver_password
  service_principal_id  = var.service_principal_id
  subscription_id       = var.subscription_id
}

module "agw_dev" {
  source                          = "./modules/application_gateway/dev"
  count                           = var.env == "dev" ? 1 : 0
  location                        = var.location
  env                             = var.env
  resource_group                  = local.resource_group
  virtual_network                 = module.virtual_network_dev[0].vnet_main_name
  key_vault_secret_id             = module.key_vault_dev[0].key_vault_secret_cert_id
  subnet_gateway_id               = module.virtual_network_dev[0].gateway_subnet_id
  sku_type                        = "WAF_v2"
  agw_identity_id                 = azurerm_user_assigned_identity.agw_identity.id
  subscription_id                 = var.subscription_id
  key_vault_name                  = module.key_vault_dev[0].key_vault_name
  agw_identity_principal_id       = azurerm_user_assigned_identity.agw_identity.principal_id
}

module "agw_test" {
  source                          = "./modules/application_gateway/test"
  count                           = var.env == "test" ? 1 : 0
  location                        = var.location
  env                             = var.env
  resource_group                  = local.resource_group
  virtual_network                 = module.virtual_network_test[0].vnet_main_name
  key_vault_secret_id             = module.key_vault_test[0].key_vault_secret_cert_id
  subnet_gateway_id               = module.virtual_network_test[0].gateway_subnet_id
  sku_type                        = "WAF_v2"
  agw_identity_id                 = azurerm_user_assigned_identity.agw_identity.id
  subscription_id                 = var.subscription_id
  key_vault_name                  = module.key_vault_test[0].key_vault_name
  agw_identity_principal_id       = azurerm_user_assigned_identity.agw_identity.principal_id
}

module "agw_prod" {
  source                          = "./modules/application_gateway/prod"
  count                           = var.env == "prod" ? 1 : 0
  location                        = var.location
  env                             = var.env
  resource_group                  = local.resource_group
  virtual_network                 = module.virtual_network_prod[0].vnet_main_name
  key_vault_secret_id             = module.key_vault_prod[0].key_vault_secret_cert_id
  subnet_gateway_id               = module.virtual_network_prod[0].gateway_subnet_id
  sku_type                        = "WAF_v2"
  agw_identity_id                 = azurerm_user_assigned_identity.agw_identity.id
  subscription_id                 = var.subscription_id
  key_vault_name                  = module.key_vault_prod[0].key_vault_name
  agw_identity_principal_id       = azurerm_user_assigned_identity.agw_identity.principal_id
}

module "aks_dev" {
  source              = "./modules/kubernetes/dev"
  count               = var.env == "dev" ? 1 : 0
  location            = var.location
  env                 = var.env
  resource_group      = local.resource_group
  tenant_id           = var.tenant_id
  dev_team_id         = var.dev_team_id
  client_id           = var.client_id
  client_secret       = var.client_secret
  aks_subnet_id       = module.virtual_network_dev[0].aks_subnet_id
  managed_identity_id = azurerm_user_assigned_identity.aks_identity.id
  subscription_id     = var.subscription_id
  virtual_network     = module.virtual_network_dev[0].vnet_main_name
  msi_id              = azurerm_user_assigned_identity.aks_identity.principal_id
  acr_id              = data.azurerm_container_registry.acr.id
  gateway_id          = module.agw_dev[0].gateway_id
  agw_snet_id         = module.virtual_network_dev[0].snet_gateway_id
}

module "aks_test" {
  source              = "./modules/kubernetes/test"
  count               = var.env == "test" ? 1 : 0
  location            = var.location
  env                 = var.env
  resource_group      = local.resource_group
  tenant_id           = var.tenant_id
  dev_team_id         = var.dev_team_id
  client_id           = var.client_id
  client_secret       = var.client_secret
  aks_subnet_id       = module.virtual_network_test[0].aks_subnet_id
  managed_identity_id = azurerm_user_assigned_identity.aks_identity.id
  subscription_id     = var.subscription_id
  virtual_network     = module.virtual_network_test[0].vnet_main_name
  msi_id              = azurerm_user_assigned_identity.aks_identity.principal_id
  acr_id              = data.azurerm_container_registry.acr.id

}

module "aks_prod" {
  source                  = "./modules/kubernetes/prod"
  count                   = var.env == "prod" ? 1 : 0
  location                = var.location
  env                     = var.env
  resource_group          = "rg-mosaico-prod"
  tenant_id               = var.tenant_id
  dev_team_id             = var.dev_team_id
  client_id               = var.client_id
  client_secret           = var.client_secret
  aks_subnet_id           = module.virtual_network_prod[0].aks_virtual_nodes_subnet_id
  virtual_nodes_subnet_id = module.virtual_network_prod[0].aks_subnet_id #ONLY ON PROD aks_subnet_id, for virtual_nodesaks_virtual_nodes_subnet_id
  managed_identity_id     = azurerm_user_assigned_identity.aks_identity.id
  subscription_id         = var.subscription_id
  virtual_network         = module.virtual_network_prod[0].vnet_main_name
  msi_id                  = azurerm_user_assigned_identity.aks_identity.principal_id
  acr_id                  = data.azurerm_container_registry.acr.id
  gateway_id              = module.agw_prod[0].gateway_id
  agw_snet_id             = module.virtual_network_prod[0].snet_gateway_id
  k8s_version             = "1.21.9"
  service_principal_id    = var.service_principal_id
}

# module "redis_dev" {
#   source         = "./modules/redis/dev"
#   count          = var.env == "dev" ? 1 : 0
#   location       = var.location
#   env            = var.env
#   resource_group = local.resource_group
#   subnet_id      = module.virtual_network_dev[0].default_subnet_id
# }

# module "redis_test" {
#   source         = "./modules/redis/test"
#   count          = var.env == "test" ? 1 : 0
#   location       = var.location
#   env            = var.env
#   resource_group = local.resource_group
#   subnet_id      = module.virtual_network_test[0].default_subnet_id
# }

# module "redis_prod" {
#   source                    = "./modules/redis/prod"
#   count                     = var.env == "prod" ? 1 : 0
#   location                  = var.location
#   env                       = var.env
#   resource_group            = local.resource_group
#   subnet_id                 = module.virtual_network_prod[0].default_subnet_id
# }

# module "frontend" {
#     source  = "./modules/cdn-static-website/dev"
#     count                           = var.env == "dev" ? 1 : 0
#     resource_group = local.resource_group
#     location = var.location
#     storage_account_name = "stfrontendmosaico${var.env}"
#     subscription_id = var.subscription_id
#     env = var.env
#     st_primary_connection_string = module.storage_account_dev[0].primary_connection_string
#     primary_web_host = module.storage_account_dev[0].primary_web_host
# }

module "alert_dev" {
  source                    = "./modules/alerts/dev"
  count                     = var.env == "dev" ? 1 : 0
  location                  = var.location
  env                       = var.env
  resource_group            = local.resource_group
  gateway_id                = module.agw_dev[0].gateway_id
}
module "alert_prod" {
  source                    = "./modules/alerts/prod"
  count                     = var.env == "prod" ? 1 : 0
  location                  = var.location
  env                       = var.env
  resource_group            = local.resource_group
}

# module "front_door_test" {
#   source                    = "./modules/front_door/test"
#   count                     = var.env == "test" ? 1 : 0
#   resource_group            = local.resource_group
#   env                       = var.env
# }

module "signalr_dev" {
  source = "./modules/signalr/dev"
  count                     = var.env == "dev" ? 1 : 0
  resource_group            = local.resource_group
  env                       = var.env
  location                  = var.location
}

module "signalr_test" {
  source = "./modules/signalr/test"
  count                     = var.env == "test" ? 1 : 0
  resource_group            = local.resource_group
  env                       = var.env
  location                  = var.location
}

module "signalr_prod" {
  source = "./modules/signalr/prod"
  count                     = var.env == "prod" ? 1 : 0
  resource_group            = local.resource_group
  env                       = var.env
  location                  = var.location
}

# module "front_door_dev" {
#   source                    = "./modules/front_door/dev"
#   count                     = var.env == "dev" ? 1 : 0
#   resource_group            = local.resource_group
#   env                       = var.env
# }

# module "front_door_test" {
#   source                    = "./modules/front_door/test"
#   count                     = var.env == "test" ? 1 : 0
#   resource_group            = local.resource_group
#   env                       = var.env
# }

module "grafana_role_dev" {
  source                    = "./modules/grafana_roles"
  count                     = var.env == "dev" ? 1 : 0
  resource_group            = local.resource_group
  subscription_id           = var.subscription_id
  service_principal_id      = var.service_principal_id
}

module "grafana_role_prod" {
  source                    = "./modules/grafana_roles"
  count                     = var.env == "prod" ? 1 : 0
  resource_group            = local.resource_group
  subscription_id           = var.subscription_id
  service_principal_id      = var.service_principal_id
}

module "application_insights_dev" {
  source                    = "./modules/application_insights/dev"
  count                     = var.env == "dev" ? 1 : 0
  resource_group            = local.resource_group
  env                       = var.env
  location                  = var.location
}

module "application_insights_prod" {
  source                    = "./modules/application_insights/prod"
  count                     = var.env == "prod" ? 1 : 0
  resource_group            = local.resource_group
  env                       = var.env
  location                  = var.location
}

