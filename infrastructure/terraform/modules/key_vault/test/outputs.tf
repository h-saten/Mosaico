output "key_vault_secret_cert_id" {
  description = "Self signed key vault certificate name"
  value       = azurerm_key_vault_certificate.cert.secret_id
}

output "key_vault_id" {
  description = "Key Vault ID"
  value       = azurerm_key_vault_certificate.cert.secret_id
}

output "key_vault_name" {
  description = "Key Vault ID"
  value       = azurerm_key_vault.key_vault.name
}
