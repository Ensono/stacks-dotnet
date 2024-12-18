locals {
  # CosmosDB
  lookup_cosmosdb_account    = !var.create_cosmosdb && var.create_function_app && var.cosmosdb_account_name != "" && var.cosmosdb_resource_group_name != ""
  cosmosdb_account_name      = var.create_cosmosdb ? module.app.cosmosdb_database_name : var.cosmosdb_account_name
  cosmosdb_connection_string = local.lookup_cosmosdb_account ? "AccountEndpoint=${data.azurerm_cosmosdb_account.cosmosdb[0].endpoint};AccountKey=${data.azurerm_cosmosdb_account.cosmosdb[0].primary_key};" : "AccountEndpoint=${module.app.cosmosdb_endpoint};AccountKey=${module.app.cosmosdb_primary_master_key};"
  cosmosdb_endpoint          = local.lookup_cosmosdb_account ? data.azurerm_cosmosdb_account.cosmosdb[0].endpoint : module.app.cosmosdb_endpoint
  # Service Bus
  lookup_servicebus_namespace = !var.create_sb_namespace && (var.create_sb_topic || var.create_sb_subscription)
  lookup_servicebus_topic     = !var.create_sb_topic && var.create_sb_subscription
  sb_topic_id                 = local.lookup_servicebus_topic ? data.azurerm_servicebus_topic.sb_topic[0].id : (var.create_sb_topic ? module.servicebus[0].servicebus_topic_id : null)
  sb_topic_name               = local.lookup_servicebus_topic ? var.sb_topic_name : (var.create_sb_topic ? module.servicebus[0].servicebus_topic_name : null)
  sb_connection_string        = local.lookup_servicebus_namespace ? data.azurerm_servicebus_namespace.sb[0].default_primary_connection_string : (var.create_sb_namespace ? module.servicebus[0].servicebus_connectionstring : null)
  # Event Hub
  lookup_eventhub             = !var.create_eventhub && var.create_function_app && var.eventhub_namespace != "" && var.eventhub_resource_group_name != "" && var.eventhub_sa_name != ""
  eventhub_connection_string  = local.lookup_eventhub ? data.azurerm_eventhub_namespace.eventhub_namespace[0].default_primary_connection_string : (var.create_eventhub ? module.eventhub[0].eventhub_connectionstring : null)
  eventhub_name               = local.lookup_eventhub ? var.eventhub_name : (var.create_eventhub ? module.eventhub[0].eventhub_name : null)
  eventhub_storage_access_key = local.lookup_eventhub ? data.azurerm_storage_account.eventhub_storage_account[0].primary_access_key : (var.create_eventhub ? module.eventhub[0].eventhub_sa_connectionstring : null)
  eventhub_blob_container     = local.lookup_eventhub ? var.eventhub_sa_blob_container : (var.create_eventhub ? module.eventhub[0].eventhub_sa_container : null)
}