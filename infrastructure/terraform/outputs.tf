output "identity_resource_id" {
    value = azurerm_user_assigned_identity.agw_identity.id
}

output "identity_client_id" {
    value = azurerm_user_assigned_identity.agw_identity.client_id
}

output "principal_client_id" {
    value = azurerm_user_assigned_identity.agw_identity.principal_id
}

# data "azurerm_kubernetes_cluster" "example" {
#   name                = "aks-mosaico-dev"
#   resource_group_name = "rg-mosaico-dev"
# }


# data "azurerm_resources" "aks-cluster-vmss" {
#   resource_group_name = "MC_rg-mosaico-dev_aks-mosaico-dev_westeurope"
#   type                = "Microsoft.Compute/virtualMachineScaleSets"
# }

# output "sadasdsda" {
#     value = data.azurerm_resources.aks-cluster-vmss.id
# }