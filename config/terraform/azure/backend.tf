terraform {
  backend "azurerm" {
    resource_group_name = "tfstate"
    container_name      = "tfstate"
    key                 = "amido/nodejs/react"
  }
}
