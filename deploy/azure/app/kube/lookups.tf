data "azurerm_client_config" "current" {}

data "terraform_remote_state" "core" {
  backend = "azurerm"

  config = {
    key                  = "${var.tfstate_key}:${var.core_environment}"
    storage_account_name = var.tfstate_storage_account
    container_name       = var.tfstate_container_name
    resource_group_name  = var.core_resource_group
  }
}

data "azurerm_application_insights" "example" {
  name                = data.terraform_remote_state.core.outputs.app_insights_name
  resource_group_name = data.terraform_remote_state.core.outputs.resource_group_name
}

data "azurerm_servicebus_namespace" "sb" {
  count               = var.create_function_app ? 1 : 0
  name                = var.sb_name
  resource_group_name = var.sb_resource_group_name
}

data "azurerm_cosmosdb_account" "cosmosdb" {
  count               = !var.create_cosmosdb && var.create_function_app ? 1 : 0
  name                = var.cosmosdb_account_name
  resource_group_name = var.cosmosdb_resource_group_name
}