resource "azurerm_frontdoor" "example" {
  name                = "fr-endpoint-${var.env}"
  resource_group_name = var.resource_group

  routing_rule {
    name               = "default-route"
    accepted_protocols = ["Http", "Https"]
    patterns_to_match  = ["/id/*","/id","/*","/"]
    frontend_endpoints = ["fr-endpoint-${var.env}"]
    forwarding_configuration {
      forwarding_protocol = "HttpOnly"
      backend_pool_name   = "default-mosaico-id"
      custom_forwarding_path = "/"
    }
  }
  
  routing_rule {
    name               = "redirect-core"
    accepted_protocols = ["Http", "Https"]
    patterns_to_match  = ["/core/*"]
    frontend_endpoints = ["fr-endpoint-${var.env}",]
    forwarding_configuration {
      forwarding_protocol = "HttpOnly"
      backend_pool_name   = "default-mosaico-core"
    }
  }

  backend_pool_load_balancing {
    name = "lb-settings"
    successful_samples_required = 3
    sample_size = 4
    additional_latency_milliseconds = 50
  }

  backend_pool_health_probe {
    name = "exampleHealthProbeSetting1"
    path = "/api/healthz"
  }

  backend_pool_settings {
    enforce_backend_pools_certificate_name_check = true
  }
  

  backend_pool {
    name = "default-mosaico-id"
    backend {
      host_header = "20.56.192.70"
      address     = "20.56.192.70"
      http_port   = 80
      https_port  = 443
    }
    
    load_balancing_name = "lb-settings"
    health_probe_name   = "exampleHealthProbeSetting1"
  }

  backend_pool {
    name = "default-mosaico-core"
    backend {
      host_header = "51.144.63.117"
      address     = "51.144.63.117"
      http_port   = 80
      https_port  = 443
    }

    load_balancing_name = "lb-settings"
    health_probe_name   = "exampleHealthProbeSetting1"

  }

  frontend_endpoint {
    name      = "fr-endpoint-${var.env}"
    host_name = "fr-endpoint-${var.env}.azurefd.net"
  }
}

resource "azurerm_frontdoor_custom_https_configuration" "example_custom_https_1" {
  frontend_endpoint_id              = azurerm_frontdoor.example.frontend_endpoints["fr-endpoint-${var.env}"]
  custom_https_provisioning_enabled = true

  custom_https_configuration {
    certificate_source                      = "FrontDoor"
  }
}

