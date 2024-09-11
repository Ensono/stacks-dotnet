# The app plans for the functions
resource "azurerm_app_service_plan" "app_sp" {
  name                = var.app_service_plan_name
  resource_group_name = var.resource_group_name
  location            = var.resource_group_location
  kind                = "linux"
  reserved            = true

  sku {
    tier = "Standard"
    size = "S1"
  }
}

# Create a new storage accounts to store future deployments
resource "azurerm_storage_account" "function" {
  name                = "${var.function_name}-${random_string.seed.result}"
  resource_group_name = var.resource_group_name
  location            = var.resource_group_location

  account_tier             = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_linux_function_app" "function" {
  name                        = "${var.function_name}-${random_string.seed.result}"
  resource_group_name         = var.resource_group_name
  location                    = var.resource_group_location
  service_plan_id             = azurerm_app_service_plan.app_sp.id
  storage_account_name        = azurerm_storage_account.function.name
  storage_account_access_key  = azurerm_storage_account.function.primary_access_key
  functions_extension_version = var.az_function_extension_version

  app_settings = {
    COSMOSDB_COLLECTION_NAME       = var.cosmosdb_collection_name
    COSMOSDB_CONNECTIONSTRING      = var.cosmosdb_connection_string
    COSMOSDB_DATABASE_NAME         = var.cosmosdb_database_name
    COSMOSDB_LEASE_COLLECTION_NAME = var.cosmosdb_lease_collection_name
    SERVICEBUS_CONNECTIONSTRING    = var.servicebus_connection_string
    TOPIC_NAME                     = var.sb_topic_name
    SUBSCRIPTION_NAME              = var.sb_subscription_name
  }

  site_config {
    always_on = true
    application_stack {
      dotnet_version = var.az_function_dotnet_version
    }
  }
}
