### Public IP ###

resource "azurerm_public_ip" "pip" {
  name                = "pip-mosaico-${var.env}"
  resource_group_name = var.resource_group
  location            = var.location
  allocation_method   = "Static"
  sku                 = "Standard"

}

locals {
  backend_address_pool_name      = "defaultaddresspool"
  frontend_port_name             = "${var.virtual_network}-feport"
  frontend_ip_configuration_name = "${var.virtual_network}-feip"
  http_setting_name              = "${var.virtual_network}-be-htst"
  listener_name                  = "${var.virtual_network}-httplstn"
  request_routing_rule_name      = "${var.virtual_network}-rqrt"
  redirect_configuration_name    = "${var.virtual_network}-rdrcfg"
}

###    MSI ROLE ASSIGNMENTS    ###
resource "azurerm_role_assignment" "agw_assignment" {
  principal_id         = var.agw_identity_principal_id            # Object ID
  role_definition_name = "Key Vault Certificates Officer"#"Key Vault Reader" 
  scope                = "/subscriptions/${var.subscription_id}/resourceGroups/${var.resource_group}/providers/Microsoft.KeyVault/vaults/${var.key_vault_name}"
}

resource "azurerm_role_assignment" "agw_secrets_officer" {
  principal_id         = var.agw_identity_principal_id            # Object ID
  role_definition_name = "Key Vault Secrets Officer"#"Key Vault Reader" 
  scope                = "/subscriptions/${var.subscription_id}/resourceGroups/${var.resource_group}/providers/Microsoft.KeyVault/vaults/${var.key_vault_name}"
}

resource "azurerm_role_assignment" "Contributor_for_kv" {
  principal_id         = var.agw_identity_principal_id            # Object ID
  role_definition_name = "Contributor"#"Key Vault Reader" 
  scope                = "/subscriptions/${var.subscription_id}/resourceGroups/${var.resource_group}/providers/Microsoft.KeyVault/vaults/${var.key_vault_name}"
}

resource "azurerm_role_assignment" "Reader_for_kv" {
  principal_id         = var.agw_identity_principal_id 
  role_definition_name = "Reader"
  scope                = "/subscriptions/${var.subscription_id}/resourceGroups/${var.resource_group}/providers/Microsoft.KeyVault/vaults/${var.key_vault_name}"
}

resource "azurerm_role_assignment" "Managed_id_operator_for_whole_rg" { #for azure ingress kubernetes
  principal_id         = var.agw_identity_principal_id         
  role_definition_name = "Managed Identity Operator" 
  scope                = "/subscriptions/${var.subscription_id}/resourceGroups/${var.resource_group}" #check if we somehow can reduce this scope to not assign on whole scope
}

resource "azurerm_role_assignment" "Reader_for_agw_msi_on_whole_rg" { #for azure ingress kubernetes
  principal_id         = var.agw_identity_principal_id      
  role_definition_name = "Reader"
  scope                = "/subscriptions/${var.subscription_id}/resourceGroups/${var.resource_group}"
}

resource "azurerm_role_assignment" "Contributor_for_agw-msi_on_AGW_scope" { #for azure ingress kubernetes
  principal_id         = var.agw_identity_principal_id      
  role_definition_name = "Contributor"
  scope                = "/subscriptions/${var.subscription_id}/resourceGroups/${var.resource_group}/providers/Microsoft.Network/applicationGateways/${azurerm_application_gateway.gateway.name}"
}
### LOGS ###

data "azurerm_log_analytics_workspace" "example" {
  name                = "log-mosaico-${var.env}"
  resource_group_name = var.resource_group
}

resource "azurerm_monitor_diagnostic_setting" "example" {
  name               = "agw-diagnostic-setting"
  target_resource_id = azurerm_application_gateway.gateway.id
  log_analytics_workspace_id = data.azurerm_log_analytics_workspace.example.id
  log {
    category = "ApplicationGatewayPerformanceLog"
    enabled  = true

    retention_policy {
      enabled = true
    }
  }
  log {
    category = "ApplicationGatewayAccessLog"
    enabled  = true

    retention_policy {
      enabled = true
    }
  }
  log {
    category = "ApplicationGatewayFirewallLog"
    enabled  = true

    retention_policy {
      enabled = true
    }
  }

  metric {
    category = "AllMetrics"

    retention_policy {
      enabled = true
    }
  }
}
### /LOGS ###

### Application Gateway ###

resource "azurerm_application_gateway" "gateway" {
  name                = "agw-mosaico-${var.env}"
  resource_group_name = var.resource_group
  location            = var.location
  enable_http2        = true
  
  lifecycle {
    ignore_changes = [
      tags,
      ssl_certificate,
      trusted_root_certificate,
      frontend_port,
      backend_address_pool,
      backend_http_settings,
      http_listener,
      url_path_map,
      request_routing_rule,
      probe,
      redirect_configuration,
      ssl_policy,
    ]
  }

  sku {
    name     = var.sku_type            
    tier     = var.sku_type                 
  } 

  gateway_ip_configuration {
    name      = "gateway-ip-config-${var.env}"
    subnet_id = var.subnet_gateway_id
  }

  identity {
    type         = "UserAssigned"
    identity_ids = [var.agw_identity_id]
  }

  waf_configuration {
    enabled = true
    rule_set_version = 3.1
    firewall_mode = "Prevention"
    disabled_rule_group {
      rule_group_name = "REQUEST-931-APPLICATION-ATTACK-RFI"
    }
    disabled_rule_group {
      rule_group_name = "REQUEST-942-APPLICATION-ATTACK-SQLI"
      rules = [942130,942430] 
    }
  }

  autoscale_configuration {
    min_capacity = 1
    max_capacity = 10
  }

  frontend_port {
    name = "${local.frontend_port_name}-80"
    port = 80
  }

  frontend_port {
    name = "${local.frontend_port_name}-443"
    port = 443
  }
  
  ssl_certificate {
    name                = "ssl"
    key_vault_secret_id = var.key_vault_secret_id
  }

  frontend_ip_configuration {
    name                 = "${local.frontend_ip_configuration_name}-public"
    public_ip_address_id = azurerm_public_ip.pip.id #"pip-mosaico-${var.env}" 
  }

    backend_address_pool {
    name = local.backend_address_pool_name
    }

    backend_http_settings {
    name                  = local.http_setting_name
    cookie_based_affinity = "Enabled"
    port                  = 80
    protocol              = "Http"
    request_timeout       = 1
    }

    http_listener {
    name                           = "https"
    frontend_ip_configuration_name = "${local.frontend_ip_configuration_name}-public"
    frontend_port_name             = "${local.frontend_port_name}-443"
    protocol                       = "Https"
    ssl_certificate_name           = "ssl"
    }

    http_listener {
    name                           = "http"
    frontend_ip_configuration_name = "${local.frontend_ip_configuration_name}-public"
    frontend_port_name             = "${local.frontend_port_name}-80"
    protocol                       = "Http"
    }


    redirect_configuration {
      name = local.redirect_configuration_name
      redirect_type = "Permanent"
      target_listener_name = "https"
      include_path = true
      include_query_string = true
    }

    request_routing_rule {
    name                       = local.request_routing_rule_name
    rule_type                  = "Basic"
    http_listener_name         = "http"
    redirect_configuration_name = local.redirect_configuration_name
    }

    request_routing_rule {
    name                       = "${local.request_routing_rule_name}-https"
    rule_type                  = "Basic"
    http_listener_name         = "https"
    backend_address_pool_name  = local.backend_address_pool_name
    backend_http_settings_name = local.http_setting_name
    }
}