data "azurerm_client_config" "current" {}

data "terraform_remote_state" "core" {
  backend = "azurerm"

  config = {
    key                  = "${var.tfstate_key}:${var.core_environment}"
    storage_account_name = var.tfstate_storage_account
    container_name       = var.tfstate_container_name
    resource_group_name  = var.tfstate_resource_group_name
  }
}

data "azurerm_application_insights" "example" {
  name                = data.terraform_remote_state.core.outputs.app_insights_name
  resource_group_name = data.terraform_remote_state.core.outputs.resource_group_name
}

data "azurerm_servicebus_namespace" "sb" {
  count               = local.lookup_servicebus_namespace ? 1 : 0
  name                = var.sb_name
  resource_group_name = var.sb_resource_group_name
}

data "azurerm_servicebus_topic" "sb_topic" {
  count        = local.lookup_servicebus_topic ? 1 : 0
  name         = var.sb_topic_name
  namespace_id = data.azurerm_servicebus_namespace.sb[0].id
}

data "azurerm_cosmosdb_account" "cosmosdb" {
  count               = local.lookup_cosmosdb_account ? 1 : 0
  name                = var.cosmosdb_account_name
  resource_group_name = var.cosmosdb_resource_group_name
}

data "azurerm_eventhub_namespace" "eventhub_namespace" {
  count               = local.lookup_eventhub ? 1 : 0
  name                = var.eventhub_namespace
  resource_group_name = var.eventhub_resource_group_name
}

data "azurerm_storage_account" "eventhub_storage_account" {
  count               = local.lookup_eventhub ? 1 : 0
  name                = var.eventhub_sa_name
  resource_group_name = var.eventhub_resource_group_name
}
