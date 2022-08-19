# terraform {
#   required_providers {
#     cloudflare = {
#       source = "cloudflare/cloudflare"
#       version = ">= 3.3.0"
#     }
#   }
# }

# locals {
#   domain = "${var.env}.mosaico.ai"
#   cdn_profile_name = "cdnp-${var.env}"
#   cdn_endpoint_name = "cdne-${var.env}"
#   cdn_endpoint_url  = "${local.cdn_endpoint_name}.azureedge.net"
# }