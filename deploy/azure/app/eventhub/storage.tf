resource "random_string" "eh_storage" {
  length  = 16
  upper   = false
  special = false
}

resource "random_string" "function_storage" {
  length  = 16
  upper   = false
  special = false
}

resource "azurerm_storage_account" "function_storage" {
  name                = random_string.function_storage.result
  resource_group_name = var.resource_group_name
  location            = var.resource_group_location

  account_tier             = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_storage_account" "eh_storage" {
  name                = random_string.eh_storage.result
  resource_group_name = var.resource_group_name
  location            = var.resource_group_location

  account_tier             = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_storage_container" "eh_storage_container" {
  name                  = var.eh_blob_container
  storage_account_name  = azurerm_storage_account.eh_storage.name
  container_access_type = "private"
}
