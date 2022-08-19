# resource "azurerm_frontdoor" "example" {
#   name                = "dev-mosaico"
#   resource_group_name = var.resource_group

#   routing_rule {
#     name               = "default-route"
#     accepted_protocols = ["Http", "Https"]
#     patterns_to_match  = ["/id/*","/id","/*","/"]
#     frontend_endpoints = ["dev-mosaico"]
#     forwarding_configuration {
#       forwarding_protocol = "HttpOnly"
#       backend_pool_name   = "default-mosaico-id"
#       custom_forwarding_path = "/"
#     }
#   }
  
#   routing_rule {
#     name               = "redirect-core"
#     accepted_protocols = ["Http", "Https"]
#     patterns_to_match  = ["/core/*"]
#     frontend_endpoints = ["dev-mosaico"]
#     forwarding_configuration {
#       forwarding_protocol = "HttpOnly"
#       backend_pool_name   = "default-mosaico-core"
#     }
#   }

#   backend_pool_load_balancing {
#     name = "lb-settings"
#     successful_samples_required = 3
#     sample_size = 4
#     additional_latency_milliseconds = 50
#   }

#   backend_pool_health_probe {
#     name = "exampleHealthProbeSetting1"
#     path = "/api/healthz"
#   }


#   enforce_backend_pools_certificate_name_check = true
  

#   backend_pool {
#     name = "default-mosaico-id"
#     backend {
#       host_header = "20.126.205.19"
#       address     = "20.126.205.19"
#       http_port   = 80
#       https_port  = 443
#     }
    
#     load_balancing_name = "lb-settings"
#     health_probe_name   = "exampleHealthProbeSetting1"
#   }

#   backend_pool {
#     name = "default-mosaico-core"
#     backend {
#       host_header = "20.103.193.216"
#       address     = "20.103.193.216"
#       http_port   = 80
#       https_port  = 443
#     }

#     load_balancing_name = "lb-settings"
#     health_probe_name   = "exampleHealthProbeSetting1"

#   }

#   frontend_endpoint {
#     name      = "dev-mosaico"
#     host_name = "dev-mosaico.azurefd.net"
#   }
# }

# resource "azurerm_frontdoor_custom_https_configuration" "example_custom_https_1" {
#   frontend_endpoint_id              = azurerm_frontdoor.example.frontend_endpoints["dev-mosaico"]
#   custom_https_provisioning_enabled = true

#   custom_https_configuration {
#     certificate_source   = "FrontDoor"
#   }
# }

