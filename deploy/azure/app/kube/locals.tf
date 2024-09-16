locals {
  lookup_cosmosdb_account     = !var.create_cosmosdb && var.create_function_app && var.cosmosdb_account_name != "" && var.cosmosdb_resource_group_name != ""
  cosmosdb_account_name       = var.create_cosmosdb ? module.app.cosmosdb_database_name : var.cosmosdb_account_name
  cosmosdb_connection_string  = local.lookup_cosmosdb_account ? "AccountEndpoint=${data.azurerm_cosmosdb_account.cosmosdb[0].endpoint};AccountKey=${data.azurerm_cosmosdb_account.cosmosdb[0].primary_key};" : "AccountEndpoint=${module.app.cosmosdb_endpoint};AccountKey=${module.app.cosmosdb_primary_master_key};"
  lookup_eventhub             = var.create_function_app && var.eventhub_namespace != "" && var.eventhub_resource_group_name != "" && var.eventhub_sa_name != "" && !contains(split(",", var.app_bus_type), "eventhub")
  eventhub_connection_string  = local.lookup_eventhub ? data.azurerm_eventhub_namespace.eventhub_namespace.default_primary_connection_string : module.eventhub[0].eventhub_connectionstring
  eventhub_name               = local.lookup_eventhub ? var.eventhub_name : module.eventhub[0].eventhub_name
  eventhub_storage_access_key = local.lookup_eventhub ? data.azurerm_storage_account.eventhub_storage_account.primary_access_key : module.eventhub[0].eventhub_sa_connectionstring
  eventhub_blob_container     = local.lookup_eventhub ? var.eventhub_sa_blob_container : module.eventhub[0].eventhub_sa_container
}