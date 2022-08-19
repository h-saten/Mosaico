resource "azurerm_signalr_service" "example" {
    name                        = "mosaico${var.env}"
    location                    = var.location
    resource_group_name         = var.resource_group

    sku {
        name     = "Standard_S1"
        capacity = 1
    }

  features {
    flag  = "ServiceMode"
    value = "Default"
  }
  features {
    flag  = "EnableConnectivityLogs"
    value = "True"
  }
}

