##################################################
# ResourceGroups

resource "azurerm_resource_group" "env" {
  name     = local.resource_group_name_env
  location = var.resource_group_location_env
  tags     = var.resource_group_tags
}

##################################################
# CosmosDB Resource

resource "azurerm_cosmosdb_table" "table" {
  name                = local.cosmosdb_table_name
  resource_group_name = azurerm_cosmosdb_account.db.resource_group_name
  account_name        = azurerm_cosmosdb_account.db.name
}


resource "azurerm_cosmosdb_account" "db" {
  name                = local.cosmosdb_account_name
  location            = azurerm_resource_group.env.location
  resource_group_name = azurerm_resource_group.env.name
  offer_type          = "Standard"
  kind                = "MongoDB"

  enable_automatic_failover = true

  consistency_policy {
    consistency_level       = "BoundedStaleness"
    max_interval_in_seconds = 10
    max_staleness_prefix    = 200
  }

  geo_location {
    location          = azurerm_resource_group.env.location
    failover_priority = 0
  }

}
