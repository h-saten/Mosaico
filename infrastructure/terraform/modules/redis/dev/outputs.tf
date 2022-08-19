output "connection_string" {
  description = "Redis connection string for secret"
  value       = azurerm_redis_cache.redis.primary_connection_string
}