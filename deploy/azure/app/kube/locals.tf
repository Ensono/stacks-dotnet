locals {
  # CosmosDB
  lookup_cosmosdb_account    = !var.create_cosmosdb && var.create_function_app && var.cosmosdb_account_name != "" && var.cosmosdb_resource_group_name != ""
  cosmosdb_account_name      = var.create_cosmosdb ? module.app.cosmosdb_database_name : var.cosmosdb_account_name
  cosmosdb_connection_string = local.lookup_cosmosdb_account ? "AccountEndpoint=${data.azurerm_cosmosdb_account.cosmosdb[0].endpoint};AccountKey=${data.azurerm_cosmosdb_account.cosmosdb[0].primary_key};" : "AccountEndpoint=${module.app.cosmosdb_endpoint};AccountKey=${module.app.cosmosdb_primary_master_key};"
  # Service Bus
  lookup_servicebus    = var.sb_resource_group_name != "" && !contains(split(",", var.app_bus_type), "servicebus")
  sb_topic_id          = local.lookup_servicebus ? data.azurerm_servicebus_topic.sb_topic[0].id : (contains(split(",", var.app_bus_type), "servicebus") ? module.servicebus[0].servicebus_topic_id : null)
  sb_topic_name        = local.lookup_servicebus ? var.sb_topic_name : (contains(split(",", var.app_bus_type), "servicebus") ? module.servicebus[0].servicebus_topic_name : null)
  sb_connection_string = local.lookup_servicebus ? data.azurerm_servicebus_namespace.sb[0].default_primary_connection_string : (contains(split(",", var.app_bus_type), "servicebus") ? module.servicebus[0].servicebus_connectionstring : null)
  # Event Hub
  lookup_eventhub             = var.create_function_app && var.eventhub_namespace != "" && var.eventhub_resource_group_name != "" && var.eventhub_sa_name != "" && !contains(split(",", var.app_bus_type), "eventhub")
  eventhub_connection_string  = local.lookup_eventhub ? data.azurerm_eventhub_namespace.eventhub_namespace[0].default_primary_connection_string : (contains(split(",", var.app_bus_type), "eventhub") ? module.eventhub[0].eventhub_connectionstring : null)
  eventhub_name               = local.lookup_eventhub ? var.eventhub_name : (contains(split(",", var.app_bus_type), "eventhub") ? module.eventhub[0].eventhub_name : null)
  eventhub_storage_access_key = local.lookup_eventhub ? data.azurerm_storage_account.eventhub_storage_account[0].primary_access_key : (contains(split(",", var.app_bus_type), "eventhub") ? module.eventhub[0].eventhub_sa_connectionstring : null)
  eventhub_blob_container     = local.lookup_eventhub ? var.eventhub_sa_blob_container : (contains(split(",", var.app_bus_type), "eventhub") ? module.eventhub[0].eventhub_sa_container : null)
}