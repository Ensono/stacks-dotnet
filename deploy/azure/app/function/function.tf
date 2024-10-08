resource "azurerm_service_plan" "app_sp" {
  name                = var.app_service_plan_name
  resource_group_name = var.resource_group_name
  location            = var.resource_group_location
  os_type             = "Linux"
  sku_name            = "S2"
}

resource "random_string" "storage_account_name" {
  length  = 16
  upper   = false
  special = false
}

resource "azurerm_storage_account" "function" {
  name                = random_string.storage_account_name.result
  resource_group_name = var.resource_group_name
  location            = var.resource_group_location

  account_tier             = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_linux_function_app" "function" {
  name                        = "${var.function_name}-${random_string.seed.result}"
  resource_group_name         = var.resource_group_name
  location                    = var.resource_group_location
  service_plan_id             = azurerm_service_plan.app_sp.id
  storage_account_name        = azurerm_storage_account.function.name
  storage_account_access_key  = azurerm_storage_account.function.primary_access_key
  functions_extension_version = var.az_function_extension_version

  app_settings = {
    COSMOSDB_CONTAINER_NAME        = var.cosmosdb_collection_name
    COSMOSDB_CONNECTIONSTRING      = var.cosmosdb_connection_string
    COSMOSDB_DATABASE_NAME         = var.cosmosdb_database_name
    COSMOSDB_LEASE_COLLECTION_NAME = var.cosmosdb_lease_collection_name
    SERVICEBUS_CONNECTIONSTRING    = var.sb_connection_string
    TOPIC_NAME                     = var.sb_topic_name
    SUBSCRIPTION_NAME              = var.sb_subscription_name != null && var.sb_topic_id != null ? azurerm_servicebus_subscription.sb_sub[0].name : null
    EVENTHUB_CONNECTIONSTRING      = var.eventhub_connection_string
    EVENTHUB_NAME                  = var.eventhub_name
    BLOB_STORAGE_CONNECTIONSTRING  = var.eventhub_storage_access_key
    BLOB_CONTAINER_NAME            = var.eventhub_blob_container
  }

  site_config {
    always_on = true
    application_stack {
      dotnet_version              = var.az_function_dotnet_version
      use_dotnet_isolated_runtime = var.az_function_dotnet_isolated_runtime
    }
  }
}
