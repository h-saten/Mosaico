resource "azurerm_kubernetes_cluster" "k8s" {
  name                    = "sapiency-cluster"
  location                = var.location
  resource_group_name     = var.resource_group
  dns_prefix              = "dns-aks-${var.env}" #dns prefix to use for aks cluster
  kubernetes_version      = "1.21.9"             #set as described, but when it is static defined, it wont auto-upgrade
  private_cluster_enabled = false                #access it from public internet
  sku_tier                = "Paid"
  
  # we ignore addon_profile because aks tries to kill AGIC everytime.
  lifecycle {
    ignore_changes = [
      addon_profile
    ]
  }

  default_node_pool {
    name                  = "system"
    node_count            = 1
    vm_size               = "Standard_B4ms"
    enable_node_public_ip = false
    max_pods              = 40 #this is minimal value
    vnet_subnet_id        = var.aks_subnet_id
  }

  addon_profile {

    http_application_routing {
      enabled = false
    }

    aci_connector_linux {
      enabled = false
    }    

    # ingress_application_gateway {
    #   enabled = true
    #   gateway_id = var.gateway_id
    # }
  }

  network_profile {
    load_balancer_sku  = "Standard"
    network_policy     = "azure"
    network_plugin     = "azure"
    docker_bridge_cidr = "192.168.0.1/16"
    dns_service_ip     = "10.0.0.10"
    service_cidr       = "10.0.0.0/16"
  }

  #=======RBAC BLOCK=======
  role_based_access_control {
    enabled = true
    azure_active_directory {
      managed                = true
      azure_rbac_enabled     = true
      tenant_id              = var.tenant_id
      admin_group_object_ids = [var.dev_team_id]
    }
  }

  identity {
    type                      = "UserAssigned"
    user_assigned_identity_id = var.managed_identity_id
  }
}

resource "azurerm_role_assignment" "example" {
  scope = "/subscriptions/${var.subscription_id}/resourceGroups/${azurerm_kubernetes_cluster.k8s.node_resource_group}"
  role_definition_name = "Reader"
  principal_id         = var.dev_team_id
}

# resource "azurerm_role_assignment" "example2" {
#   scope = "/subscriptions/6eb5c473-142c-4043-a506-cc94fdbedce0/resourceGroups/MC_rg-sapiency_sapiency-cluster_westeurope"
#   role_definition_name = "Owner"
#   principal_id         = var.dev_team_id
# }

resource "azurerm_container_registry" "acr" {
  name                = "acrsapiency"
  resource_group_name = "rg-sapiency"
  location            = var.location
  sku                 = "Basic"
  admin_enabled       = false
    network_rule_set = []
}

resource "azurerm_role_assignment" "kubweb_to_acr" {
  scope                = azurerm_container_registry.acr.id  
  role_definition_name = "AcrPull"
  principal_id         = azurerm_kubernetes_cluster.k8s.kubelet_identity[0].object_id #attach acr to aks, allowing it with AcrPull
}

# resource "azurerm_role_assignment" "k8s_to_loadbalancer" {
#   scope                = "/subscriptions/6eb5c473-142c-4043-a506-cc94fdbedce0/resourceGroups/rg-sapiency" 
#   role_definition_name = "Virtual Machine Administrator Login"
#   principal_id         = "a6c2660b-17c2-4c65-8bbd-aa44ce735f65"
# }

# resource "azurerm_role_assignment" "example3" {
#   scope = "/subscriptions/6eb5c473-142c-4043-a506-cc94fdbedce0/resourceGroups/MC_rg-sapiency_sapiency-cluster_westeurope"
#   role_definition_name = "Owner"
#   principal_id         = "a6c2660b-17c2-4c65-8bbd-aa44ce735f65"
# }

