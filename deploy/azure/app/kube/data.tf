data "azurerm_container_registry" "acr" {
  name                = "ensonoeuw"
  resource_group_name = "stacks-ancillary-resources"
}

data "azurerm_container_app_environment" "acae" {
  name                = "ed-stacks-nonprod-uks-aca"
  resource_group_name = "ed-stacks-nonprod-uks-aca"
}
