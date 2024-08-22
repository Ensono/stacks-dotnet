resource "random_string" "sa_name_listener" {
  length  = 16
  upper   = false
  special = false
}


// resource "azurerm_storage_account" "sa_listener" {
//   name                = random_string.sa_name_listener.result
//   resource_group_name = var.resource_group_name
//   location            = var.resource_group_location

//   account_tier             = "Standard"
//   account_replication_type = "LRS"
// }

# The app plans for the functions
resource "azurerm_app_service_plan" "app_sp" {
  name                = var.app-service-plan-name
  resource_group_name = var.resource_group_name
  location            = var.resource_group_location
  kind                = "linux"
  reserved            = true

  sku {
    tier = "Standard"
    size = "S1"
  }
}

resource "azurerm_function_app" "function_listener" {
  name                = "${var.func-aeh-listener-name}-${random_string.seed.result}"
  resource_group_name = var.resource_group_name
  location            = var.resource_group_location
  depends_on = [
    azurerm_eventhub_namespace.eh_ns
  ]

  app_service_plan_id        = azurerm_app_service_plan.app_sp.id
  storage_account_name       = azurerm_storage_account.function_storage.name
  storage_account_access_key = azurerm_storage_account.function_storage.primary_access_key

  app_settings = {
    NAMESPACE_CONNECTIONSTRING    = azurerm_eventhub_namespace.eh_ns.default_primary_connection_string
    EVENT_HUB_NAME                = azurerm_eventhub.eh.name
    BLOB_STORAGE_CONNECTIONSTRING = azurerm_storage_account.eh_storage.primary_access_key
    BLOB_CONTAINER_NAME           = azurerm_storage_container.eh_storage_container.name
  }

  version = var.az_function_extension_version

  site_config {
    always_on                = true
    dotnet_framework_version = var.az_function_dotnet_version
  }
}

