terraform {
  backend "azurerm" {
    storage_account_name = "amidostackstfstatetmp"
    resource_group_name  = "amido-stacks-infra-tfstate-rg-uks"
    container_name       = "tfstate"
    # key                  = "dev/stacks-v2"
  }
}

