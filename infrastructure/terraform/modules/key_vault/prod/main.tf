data "azurerm_client_config" "current" {} #get client config

resource "azurerm_key_vault_access_policy" "agw" {
  key_vault_id = azurerm_key_vault.key_vault.id
  tenant_id    = var.tenant_id
  object_id    = var.msi_id

  secret_permissions = [
    "get"
  ]
}
resource "azurerm_key_vault" "key_vault" {
  name                       = "kv-mosaico-v-2-${var.env}"
  location                   = var.location
  resource_group_name        = var.resource_group
  tenant_id                  = var.tenant_id
  soft_delete_retention_days = 7 #PROBABLY 90 FOR WAF V2 ON AGW, BUT CHECK IT LATER
  purge_protection_enabled   = false
  enable_rbac_authorization  = true
  sku_name                   = "standard"
  
  network_acls {
    virtual_network_subnet_ids = [var.vnet-vpnhub-shared_default, var.vnet-mosaico-dev_snet_default, var.vnet-vpnhub-shared_gateway_subnet, var.vnet-mosaico-dev_snet_aks_virtual_nodes, var.vnet-mosaico-dev_snet_aks]
    bypass = "AzureServices"
    default_action = "Deny"
  }
  lifecycle {
    ignore_changes = [network_acls]
  }
}

#get higher privilege to assign that role
resource "azurerm_role_assignment" "sp_assignment" {
  scope                = azurerm_key_vault.key_vault.id
  role_definition_name = "Key Vault Secrets Officer"
  principal_id         = var.service_principal_id
}

resource "azurerm_role_assignment" "devgr_assignment" {
  scope                = azurerm_key_vault.key_vault.id
  role_definition_name = "Key Vault Secrets Officer"
  principal_id         = var.dev_team_id
}

resource "azurerm_role_assignment" "dev_cert_assign" {
  scope                = azurerm_key_vault.key_vault.id
  role_definition_name = "Key Vault Certificates Officer"
  principal_id         = var.dev_team_id
}

resource "azurerm_role_assignment" "sp_cert_assign" {
  scope                = azurerm_key_vault.key_vault.id
  role_definition_name = "Key Vault Certificates Officer"
  principal_id         = var.service_principal_id
}

# ============PrivateLink=========================
resource "azurerm_private_dns_zone" "dns_zone" {
  name                = "privatelink.vaultcore.azure.net"
  resource_group_name = var.resource_group
}

resource "azurerm_private_dns_zone_virtual_network_link" "dns_zone_vn_link" {
  name                  = "pl-vnet-dns" #not sure if that link is necessary
  resource_group_name   = var.resource_group
  private_dns_zone_name = azurerm_private_dns_zone.dns_zone.name
  virtual_network_id    = var.vnet_main_id
}

resource "azurerm_private_endpoint" "private_endpoint" { 
  depends_on = [
    azurerm_key_vault.key_vault,
    azurerm_private_dns_zone.dns_zone
  ]

  name                = "pe-kv-mosaico-${var.env}"
  location            = var.location
  resource_group_name = var.resource_group
  subnet_id           = var.vnet_subnet_id

  private_dns_zone_group {
    name                 = "private-dns-zone-group"
    private_dns_zone_ids = [azurerm_private_dns_zone.dns_zone.id]
  }

  private_service_connection {
    name                           = "psc-kv-mosaico${var.env}"
    is_manual_connection           = false
    private_connection_resource_id = azurerm_key_vault.key_vault.id
    subresource_names              = ["vault"]
  }
}

resource "azurerm_private_dns_a_record" "pe_kv" {
  name                = "dns_pe_a_record"
  zone_name           = azurerm_private_dns_zone.dns_zone.name
  resource_group_name = var.resource_group
  ttl                 = 300
  records             = ["172.${var.network_prefix}.16.1"]
}
# OUR NEW CERT, DELETE SELF-SIGNED BELOW WHEN NEW IS PRESENT

# resource "azurerm_key_vault_certificate" "example" {
#   name         = "signed-cert-agw"
#   key_vault_id = azurerm_key_vault.key_vault.id

#   certificate {
#     contents = filebase64("certificate-to-import.pfx")
#     password = ""
#   }
# }
resource "azurerm_key_vault_secret" "SqlServer" {
  depends_on = [
    azurerm_role_assignment.sp_assignment,
    azurerm_role_assignment.devgr_assignment
  ]

  name         = "SqlServer--ConnectionString"
  value        = "Server=tcp:${var.sql_server_fqdn},1433;Initial Catalog=${var.sql_database_name};Persist Security Info=False;User ID=${var.sql_admin_Login};Password=${var.sql_server_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
  key_vault_id = azurerm_key_vault.key_vault.id
}


resource "azurerm_key_vault_secret" "SqlIdentityServer" {
  depends_on = [
    azurerm_role_assignment.sp_assignment,
    azurerm_role_assignment.devgr_assignment
  ]

  name         = "IdentitySqlServer--ConnectionString"
  value        = "Server=tcp:${var.sql_server_fqdn},1433;Initial Catalog=${var.sql_identity_database_name};Persist Security Info=False;User ID=${var.sql_admin_Login};Password=${var.sql_server_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"

  key_vault_id = azurerm_key_vault.key_vault.id
}

resource "azurerm_key_vault_secret" "Cache" {
  depends_on = [
    azurerm_role_assignment.sp_assignment,
    azurerm_role_assignment.devgr_assignment
  ]

  name         = "Cache--RedisConnectionString"
  value        = var.redis_primary_connection_string
  key_vault_id = azurerm_key_vault.key_vault.id
}

resource "azurerm_key_vault_secret" "EventSourcing" {
  depends_on = [
    azurerm_role_assignment.sp_assignment,
    azurerm_role_assignment.devgr_assignment
  ]

  name         = "EventSourcing--RedisConnectionString"
  value        = var.redis_primary_connection_string
  key_vault_id = azurerm_key_vault.key_vault.id
}

resource "azurerm_key_vault_secret" "ServiceBus" {
  depends_on = [
    azurerm_role_assignment.sp_assignment,
    azurerm_role_assignment.devgr_assignment
  ]

  name         = "ServiceBus--ConnectionString"
  value        = var.sb_primary_connection_string
  key_vault_id = azurerm_key_vault.key_vault.id
}

resource "azurerm_key_vault_secret" "AzureBlobStorageConnection" {
  depends_on = [
    azurerm_role_assignment.sp_assignment,
    azurerm_role_assignment.devgr_assignment
  ]

  name         = "AzureBlobStorage--ConnectionString"
  value        = var.st_primary_connection_string
  key_vault_id = azurerm_key_vault.key_vault.id
}

resource "azurerm_key_vault_secret" "AzureBlobStorageEndpoint" {
  depends_on = [
    azurerm_role_assignment.sp_assignment,
    azurerm_role_assignment.devgr_assignment
  ]
  name         = "AzureBlobStorage--EndpointUrl"
  value        = var.st_primary_endpoint
  key_vault_id = azurerm_key_vault.key_vault.id
}

resource "azurerm_key_vault_secret" "EthereumAdminAccountPrivateKey" {
  depends_on = [
    azurerm_role_assignment.sp_assignment,
    azurerm_role_assignment.devgr_assignment
  ]
  name         = "Ethereum--AdminAccount--PrivateKey"
  value        = ""
  key_vault_id = azurerm_key_vault.key_vault.id
  lifecycle {
    ignore_changes = [value]
  }
}

resource "azurerm_key_vault_secret" "app-insights-logger-instrumentation-key" {
  depends_on = [
    azurerm_role_assignment.sp_assignment,
    azurerm_role_assignment.devgr_assignment
  ]
  name         = "Loggers--ApplicationInsightsLogger--InstrumentationKey"
  value        = var.instrumentation_key
  key_vault_id = azurerm_key_vault.key_vault.id
  lifecycle {
    ignore_changes = [value]
  }
}

resource "azurerm_key_vault_secret" "Moralis" {
  depends_on = [
    azurerm_role_assignment.sp_assignment,
    azurerm_role_assignment.devgr_assignment
  ]
  name         = "Moralis--ApiKey" #lifecycle ignore
  value        = ""
  key_vault_id = azurerm_key_vault.key_vault.id
  lifecycle {
    ignore_changes = [value]
  }
}

resource "azurerm_key_vault_secret" "Hangfire" {
  depends_on = [
    azurerm_role_assignment.sp_assignment,
    azurerm_role_assignment.devgr_assignment
  ]
  name         = "Hangfire--ConnectionString"
  value        = "Server=tcp:${var.sql_server_fqdn},1433;Initial Catalog=${var.sql_database_name};Persist Security Info=False;User ID=${var.sql_admin_Login};Password=${var.sql_server_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
  key_vault_id = azurerm_key_vault.key_vault.id
}

resource "azurerm_key_vault_secret" "EmailLabsKey" {
  depends_on = [
    azurerm_role_assignment.sp_assignment,
    azurerm_role_assignment.devgr_assignment
  ]
  name         = "EmailLabs--AppKey" #lifecycle ignore
  value        = ""
  key_vault_id = azurerm_key_vault.key_vault.id
  lifecycle {
    ignore_changes = [value]
  }
}

resource "azurerm_key_vault_secret" "EmailLabsSecret" {
  depends_on = [
    azurerm_role_assignment.sp_assignment,
    azurerm_role_assignment.devgr_assignment
  ]
  name         = "EmailLabs--SecretKey"
  value        = ""
  key_vault_id = azurerm_key_vault.key_vault.id
}

resource "azurerm_key_vault_secret" "SmsLabsAppKey" {
  depends_on = [
    azurerm_role_assignment.sp_assignment,
    azurerm_role_assignment.devgr_assignment
  ]
  name         = "SmsLabs--AppKey"
  value        = ""
  key_vault_id = azurerm_key_vault.key_vault.id
  lifecycle {
    ignore_changes = [value]
  }
}

resource "azurerm_key_vault_secret" "SmsLabsSecretKey" {
  depends_on = [
    azurerm_role_assignment.sp_assignment,
    azurerm_role_assignment.devgr_assignment
  ]
  name         = "SmsLabs--SecretKey"
  value        = ""
  key_vault_id = azurerm_key_vault.key_vault.id
  lifecycle {
    ignore_changes = [value]
  }
}

resource "azurerm_key_vault_secret" "TokenizerSqlServerConnectionString" {
  depends_on = [
    azurerm_role_assignment.sp_assignment,
    azurerm_role_assignment.devgr_assignment
  ]
  name         = "TokenizerSqlServer--ConnectionString"
  value        = ""
  key_vault_id = azurerm_key_vault.key_vault.id
  lifecycle {
    ignore_changes = [value]
  }
}

resource "azurerm_key_vault_secret" "BasisIdSecret" {
  depends_on = [
    azurerm_role_assignment.sp_assignment,
    azurerm_role_assignment.devgr_assignment
  ]
  name         = "BasisId--Secret"
  value        = ""
  key_vault_id = azurerm_key_vault.key_vault.id
  lifecycle {
    ignore_changes = [value]
  }
}

resource "azurerm_key_vault_secret" "Binance--Apisecret" {
  depends_on = [
    azurerm_role_assignment.sp_assignment,
    azurerm_role_assignment.devgr_assignment
  ]
  name         = "Binance--ApiSecret"
  value        = ""
  key_vault_id = azurerm_key_vault.key_vault.id
  lifecycle {
    ignore_changes = [value]
  }
}

resource "azurerm_key_vault_secret" "Blockchain--Networks--0--AdminAccount--PrivateKey" {
  depends_on = [
    azurerm_role_assignment.sp_assignment,
    azurerm_role_assignment.devgr_assignment
  ]
  name         = "Blockchain--Networks--0--AdminAccount--PrivateKey"
  value        = ""
  key_vault_id = azurerm_key_vault.key_vault.id
  lifecycle {
    ignore_changes = [value]
  }
}

resource "azurerm_key_vault_secret" "Blockchain--Networks--0--EtherscanApiToken" {
  depends_on = [
    azurerm_role_assignment.sp_assignment,
    azurerm_role_assignment.devgr_assignment
  ]
  name         = "Blockchain--Networks--0--EtherscanApiToken"
  value        = ""
  key_vault_id = azurerm_key_vault.key_vault.id
  lifecycle {
    ignore_changes = [value]
  }
}

resource "azurerm_key_vault_secret" "CoinAPI--ApiKey" {
  depends_on = [
    azurerm_role_assignment.sp_assignment,
    azurerm_role_assignment.devgr_assignment
  ]
  name         = "CoinAPI--ApiKey"
  value        = ""
  key_vault_id = azurerm_key_vault.key_vault.id
  lifecycle {
    ignore_changes = [value]
  }
}

resource "azurerm_key_vault_secret" "Hangfire--AccessPassword" {
  depends_on = [
    azurerm_role_assignment.sp_assignment,
    azurerm_role_assignment.devgr_assignment
  ]
  name         = "Hangfire--AccessPassword"
  value        = ""
  key_vault_id = azurerm_key_vault.key_vault.id
  lifecycle {
    ignore_changes = [value]
  }
}

resource "azurerm_key_vault_secret" "Kanga--Api--AppId" {
  depends_on = [
    azurerm_role_assignment.sp_assignment,
    azurerm_role_assignment.devgr_assignment
  ]
  name         = "Kanga--Api--AppId"
  value        = ""
  key_vault_id = azurerm_key_vault.key_vault.id
  lifecycle {
    ignore_changes = [value]
  }
}

resource "azurerm_key_vault_secret" "Kanga--Api--AppSecret" {
  depends_on = [
    azurerm_role_assignment.sp_assignment,
    azurerm_role_assignment.devgr_assignment
  ]
  name         = "Kanga--Api--AppSecret"
  value        = ""
  key_vault_id = azurerm_key_vault.key_vault.id
  lifecycle {
    ignore_changes = [value]
  }
}

resource "azurerm_key_vault_secret" "Passbase--ApiSecret" {
  depends_on = [
    azurerm_role_assignment.sp_assignment,
    azurerm_role_assignment.devgr_assignment
  ]
  name         = "Passbase--ApiSecret"
  value        = ""
  key_vault_id = azurerm_key_vault.key_vault.id
  lifecycle {
    ignore_changes = [value]
  }
}

resource "azurerm_key_vault_secret" "RabbitMQ--Host" {
  depends_on = [
    azurerm_role_assignment.sp_assignment,
    azurerm_role_assignment.devgr_assignment
  ]
  name         = "RabbitMQ--Host"
  value        = ""
  key_vault_id = azurerm_key_vault.key_vault.id
  lifecycle {
    ignore_changes = [value]
  }
}

resource "azurerm_key_vault_secret" "SendGridEmail--AppKey" {
  depends_on = [
    azurerm_role_assignment.sp_assignment,
    azurerm_role_assignment.devgr_assignment
  ]
  name         = ""
  value        = ""
  key_vault_id = azurerm_key_vault.key_vault.id
  lifecycle {
    ignore_changes = [value]
  }
}

resource "azurerm_key_vault_secret" "Signalr--ConnectionString" {
  depends_on = [
    azurerm_role_assignment.sp_assignment,
    azurerm_role_assignment.devgr_assignment
  ]
  name         = ""
  value        = ""
  key_vault_id = azurerm_key_vault.key_vault.id
  lifecycle {
    ignore_changes = [value]
  }
}

resource "azurerm_key_vault_secret" "Transak--ApiSecret" {
  depends_on = [
    azurerm_role_assignment.sp_assignment,
    azurerm_role_assignment.devgr_assignment
  ]
  name         = "Transak--ApiSecret"
  value        = ""
  key_vault_id = azurerm_key_vault.key_vault.id
  lifecycle {
    ignore_changes = [value]
  }
}

resource "azurerm_key_vault_secret" "UserCom--AuthorizationToken" {
  depends_on = [
    azurerm_role_assignment.sp_assignment,
    azurerm_role_assignment.devgr_assignment
  ]
  name         = "UserCom--AuthorizationToken"
  value        = ""
  key_vault_id = azurerm_key_vault.key_vault.id
  lifecycle {
    ignore_changes = [value]
  }
}

data "azurerm_key_vault_secret" "_1" {
  name         = "SqlServer--ConnectionString"
  key_vault_id = azurerm_key_vault.key_vault.id
  depends_on = [
    azurerm_key_vault_secret.SqlServer
  ]
}

data "azurerm_key_vault_secret" "_2" {
  name         = "Cache--RedisConnectionString"
  key_vault_id = azurerm_key_vault.key_vault.id
  depends_on = [
    azurerm_key_vault_secret.Cache
  ]
}

data "azurerm_key_vault_secret" "_3" {
  name         = "EventSourcing--RedisConnectionString"
  key_vault_id = azurerm_key_vault.key_vault.id
  depends_on = [
    azurerm_key_vault_secret.EventSourcing
  ]
}

data "azurerm_key_vault_secret" "_4" {
  name         = "ServiceBus--ConnectionString"
  key_vault_id = azurerm_key_vault.key_vault.id
  depends_on = [
    azurerm_key_vault_secret.ServiceBus
  ]
}

data "azurerm_key_vault_secret" "_5" {
  name         = "AzureBlobStorage--ConnectionString"
  key_vault_id = azurerm_key_vault.key_vault.id
  depends_on = [
    azurerm_key_vault_secret.AzureBlobStorageConnection
  ]
}

data "azurerm_key_vault_secret" "_6" {
  name         = "AzureBlobStorage--EndpointUrl"
  key_vault_id = azurerm_key_vault.key_vault.id
  depends_on = [
    azurerm_key_vault_secret.AzureBlobStorageEndpoint
  ]
}

data "azurerm_key_vault_secret" "_7" {
  name         = "Loggers--ApplicationInsightsLogger--InstrumentationKey"
  key_vault_id = azurerm_key_vault.key_vault.id
  depends_on = [
    azurerm_key_vault_secret.app-insights-logger-instrumentation-key
  ]
}


data "azurerm_key_vault_secret" "_9" {
  name         = "Ethereum--AdminAccount--PrivateKey"
  key_vault_id = azurerm_key_vault.key_vault.id
  depends_on = [
    azurerm_key_vault_secret.EthereumAdminAccountPrivateKey
  ]
}

data "azurerm_key_vault_secret" "_10" {
  name         = "Moralis--ApiKey"
  key_vault_id = azurerm_key_vault.key_vault.id
  depends_on = [
    azurerm_key_vault_secret.Moralis
  ]
}

data "azurerm_key_vault_secret" "_11" {
  name         = "Hangfire--ConnectionString"
  key_vault_id = azurerm_key_vault.key_vault.id
  depends_on = [
    azurerm_key_vault_secret.Hangfire
  ]
}

data "azurerm_key_vault_secret" "_12" {
  name         = "EmailLabs--AppKey"
  key_vault_id = azurerm_key_vault.key_vault.id
  depends_on = [
    azurerm_key_vault_secret.EmailLabsKey
  ]
}

data "azurerm_key_vault_secret" "_13" {
  name         = "EmailLabs--SecretKey"
  key_vault_id = azurerm_key_vault.key_vault.id
  depends_on = [
    azurerm_key_vault_secret.EmailLabsSecret
  ]
}

data "azurerm_key_vault_certificate" "_14" {
  name      = "cert"
    key_vault_id = azurerm_key_vault.key_vault.id
  depends_on = [
    azurerm_key_vault_certificate.cert
  ]
}

resource "null_resource" "null14" {
  triggers = {
    test = data.azurerm_key_vault_certificate._14.id
  }
}


resource "null_resource" "null1" {
  triggers = {
    test = data.azurerm_key_vault_secret._1.id
  }
}

resource "null_resource" "null2" {
  triggers = {
    test = data.azurerm_key_vault_secret._2.id
  }
}

resource "null_resource" "null3" {
  triggers = {
    test = data.azurerm_key_vault_secret._3.id
  }
}

resource "null_resource" "null4" {
  triggers = {
    test = data.azurerm_key_vault_secret._4.id
  }
}

resource "null_resource" "null5" {
  triggers = {
    test = data.azurerm_key_vault_secret._5.id
  }
}

resource "null_resource" "null6" {
  triggers = {
    test = data.azurerm_key_vault_secret._6.id
  }
}

resource "null_resource" "null7" {
  triggers = {
    test = data.azurerm_key_vault_secret._7.id
  }
}

resource "null_resource" "null9" {
  triggers = {
    test = data.azurerm_key_vault_secret._9.id
  }
}

resource "null_resource" "null10" {
  triggers = {
    test = data.azurerm_key_vault_secret._10.id
  }
}

resource "null_resource" "null11" {
  triggers = {
    test = data.azurerm_key_vault_secret._11.id
  }
}

resource "null_resource" "null12" {
  triggers = {
    test = data.azurerm_key_vault_secret._12.id
  }
}

resource "null_resource" "null13" {
  triggers = {
    test = data.azurerm_key_vault_secret._13.id
  }
}

#i should really used for each but it was too late
