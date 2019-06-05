resource "azurerm_resource_group" "rg" {
  name     = "${local.resource_group_name}"
  location = "${var.resource_group_location}"
  tags     = "${var.resource_group_tags}"
}

resource "azurerm_cosmosdb_account" "cosmosdb" {
  name                = "${local.cosmosdb_account_name}"
  location            = "${var.resource_group_location}"
  resource_group_name = "${azurerm_resource_group.rg.name}"
  offer_type          = "Standard"
  kind                = "MongoDB"

  consistency_policy {
    consistency_level = "Session"
  }


  failover_policy {
    location = "North Europe"
    priority = 0
  }

  tags = "${var.resource_group_tags}"
  
}