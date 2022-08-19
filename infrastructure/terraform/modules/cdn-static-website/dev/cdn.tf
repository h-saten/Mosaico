# resource "azurerm_cdn_profile" "dev_frontend" {
#   name                  = local.cdn_profile_name
#   location              = var.location
#   resource_group_name   = var.resource_group
#   sku                   = "Standard_Verizon"
# }

# resource "azurerm_cdn_endpoint" "dev_frontend" {
#   name                  = local.cdn_endpoint_name
#   profile_name          = azurerm_cdn_profile.dev_frontend.name
#   location              = var.location
#   resource_group_name   = var.resource_group

#   origin {
#     name      = "cdne-mosaico-${var.env}"
#     host_name = var.primary_web_host 
#   }
# }

# resource "azurerm_cdn_endpoint_custom_domain" "dev_frontend" {
# #   depends_on = [
# #     cloudflare_record.dev_frontend
# #   ]
#   name            = "cdne-mosaico-${var.env}"
#   cdn_endpoint_id = azurerm_cdn_endpoint.dev_frontend.id
#   host_name       = local.domain
# }

# resource "null_resource" "enable_custom_domain_https" {
#   depends_on = [
#     azurerm_cdn_endpoint_custom_domain.dev_frontend
#   ]
#   provisioner "local-exec" {
#       command = "az cdn custom-domain enable-https -g ${var.resource_group} --profile-name ${local.cdn_profile_name} --endpoint-name ${local.cdn_endpoint_name} -n ${replace(local.domain, ".", "-")} --subscription ${var.subscription_id} || true"
#   }
# }