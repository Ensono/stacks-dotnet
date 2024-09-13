locals {
  lookup_cosmosdb_account    = !var.create_cosmosdb && var.create_function_app && var.cosmosdb_account_name != "" && var.cosmosdb_resource_group_name != ""
  cosmosdb_account_name      = var.create_cosmosdb ? module.app.cosmosdb_database_name : var.cosmosdb_account_name
  cosmosdb_connection_string = local.lookup_cosmosdb_account ? "AccountEndpoint=${data.azurerm_cosmosdb_account.cosmosdb[0].endpoint};AccountKey=${data.azurerm_cosmosdb_account.cosmosdb[0].primary_key};" : "AccountEndpoint=${module.app.cosmosdb_endpoint};AccountKey=${module.app.cosmosdb_primary_master_key};"
}