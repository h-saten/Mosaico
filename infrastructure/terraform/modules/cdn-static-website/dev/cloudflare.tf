# data "cloudflare_zone" "mosaico" {
#   name = "mosaico.ai"
# }
# resource "cloudflare_record" "dev_frontend" {
#     depends_on = [
#         azurerm_cdn_endpoint.dev_frontend
#     ]
#     zone_id = data.cloudflare_zone.mosaico.id
#     name    = var.env
#     value   = local.cdn_endpoint_url
#     type    = "CNAME"
#     ttl     = 1
#     proxied = false
# }