output "key" {
  value = azurerm_cosmosdb_account.db.primary_master_key
}

output "endpoint" {
  value = azurerm_cosmosdb_account.db.endpoint
}
