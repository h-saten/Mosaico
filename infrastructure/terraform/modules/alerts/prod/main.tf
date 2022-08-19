data "azurerm_application_insights" "app_insights" {
  name                = "appi-${var.env}"
  resource_group_name = var.resource_group
}


data "azurerm_kubernetes_cluster" "example" {
  name                = "aks-mosaico-${var.env}"
  resource_group_name = var.resource_group
}

data "azurerm_log_analytics_workspace" "log_workspace" {
  name = "log-mosaico-${var.env}"
  resource_group_name = var.resource_group
}

resource "azurerm_monitor_action_group" "example" {
  name                = "CriticalAlertsAction-${var.env}"
  resource_group_name = var.resource_group
  short_name          = "alertmosaico"

  email_receiver {
    name                    = "ALERT"
    email_address           = "development@mosaico.ai"
    use_common_alert_schema = true
  }
}

resource "azurerm_monitor_scheduled_query_rules_alert" "example-alert1" {
  name                = "Unhealthy-nodes-cyclic"
  location            = var.location
  resource_group_name = var.resource_group

  action {
    action_group = [
      azurerm_monitor_action_group.example.id
    ]
  }
  data_source_id = data.azurerm_log_analytics_workspace.log_workspace.id
  description    = "Exception threshold reached"
  enabled        = true
  query = <<-QUERY
    KubeEvents
    | where TimeGenerated > ago(15m) 
    | where not(isempty(Namespace))
    | where Reason == "Unhealthy"

  QUERY
  severity    = 1
  frequency   = 15
  time_window = 15
  trigger {
    operator  = "GreaterThan"
    threshold = 0
  }
}

# resource "azurerm_monitor_scheduled_query_rules_alert" "agw-backend-health-fail" {
#   name                = "Unhealthy-backend-gateway-cyclic"
#   location            = var.location
#   resource_group_name = var.resource_group

#   action {
#     action_group = [
#       azurerm_monitor_action_group.example.id
#     ]
#   }
#   data_source_id = data.azurerm_log_analytics_workspace.log_workspace.id
#   description    = "Exception threshold reached"
#   enabled        = true
#   query = <<-QUERY
#     AzureMetrics 
#     | where TimeGenerated > ago(15m) 
#     | where MetricName == "UnhealthyHostCount"
#     | where Average > 0
#     | order by Average desc
#   QUERY

#     # AzureMetrics 
#     # | where TimeGenerated > ago(1h) 
#     # | where MetricName == "UnhealthyHostCount"
#     # | order by TimeGenerated desc
#     # | limit 1
#     # | summarize max(Count)
#   severity    = 1
#   frequency   = 15
#   time_window = 15
#   trigger {
#     operator  = "GreaterThan"
#     threshold = 0
#   }
# }

# resource "azurerm_monitor_metric_alert" "kubernetes-pods-failing-alert" {
#   name                = "pod-failure-rate"
#   resource_group_name =  var.resource_group
#   scopes              = [data.azurerm_kubernetes_cluster.example.id]
#   description         = "${title(var.env)} - Pods failing in (${data.azurerm_kubernetes_cluster.example.id})"

#   criteria {
#     metric_namespace = "Microsoft.ContainerService/managedClusters"
#     metric_name      = "kube_pod_status_phase"
#     aggregation      = "Average"
#     operator         = "GreaterThan"
#     threshold        = 1

#     dimension {
#         name     = "phase"
#         operator = "Include"
#         values   = ["Failed"]
#     }

#     dimension {
#         name     = "namespace"
#         operator = "Include"
#         values   = ["mosaico"]
#     }
#   }

#   action {
#     action_group_id = azurerm_monitor_action_group.example.id
#   }
# }

# resource "azurerm_monitor_metric_alert" "kubernetes-pods-working-alert" {
#   name                = "pod-success-rate"
#   resource_group_name =  var.resource_group
#   scopes              = [data.azurerm_kubernetes_cluster.example.id]
#   description         = "${title(var.env)} - Pods working in (${data.azurerm_kubernetes_cluster.example.id})"

#   criteria {
#     metric_namespace = "Microsoft.ContainerService/managedClusters"
#     metric_name      = "kube_pod_status_phase"
#     aggregation      = "Average"
#     operator         = "GreaterThan"
#     threshold        = 1

#     # dimension {
#     #     name     = "phase"
#     #     operator = "Include"
#     #     values   = ["Failed"]
#     # }

#     dimension {
#         name     = "namespace"
#         operator = "Include"
#         values   = ["mosaico"]
#     }
#   }

#   action {
#     action_group_id = azurerm_monitor_action_group.example.id
#   }
# }

# resource "azurerm_monitor_metric_alert" "appgw-health-fail" {
#   name                = "appgw-health-fail"
#   resource_group_name =  var.resource_group
#   scopes              = [var.gateway_id]
#   description         = "${title(var.env)} - Health status in (AGW)"

#   criteria {
#     metric_namespace = "Microsoft.Network/applicationGateways"
#     metric_name      = "HealthyHostCount"
#     aggregation      = "Average"
#     operator         = "LessThan"
#     threshold        = 2
  
#     dimension {
#         name     = "BackendSettingsPool"
#         operator = "Include"
#         values   = ["*"]
#     }
#   }

#   action {
#     action_group_id = azurerm_monitor_action_group.example.id
#   }

# }