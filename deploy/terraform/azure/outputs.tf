output "cosmosdb_rg_name" {
  value = azurerm_resource_group.env.name
}
output "cosmosdb_database_name" {
  value = azurerm_cosmosdb_sql_database.db.name
}
output "cosmosdb_account_name" {
  value = azurerm_cosmosdb_sql_database.db.account_name
}
