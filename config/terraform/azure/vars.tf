############################################
# AUTHENTICATION
############################################
variable "client_id" {}

variable "client_secret" {}
variable "subscription_id" {}
variable "tenant_id" {}

############################################
# NAMING
############################################

variable "name_company" {
  default = "amido"
}

variable "name_environment" {
  default = "dev"
}

variable "name_platform" {
  default = "dotnet"
}

variable "name_project" {
  default = "menu"
}

# Each region must have corresponding a shortend name for resource naming purposes 
variable "cluster_ingress_address" {
  type = "map"

  default = {}
}

############################################
# RESOURCE GROUP INFORMATION
############################################

variable "resource_group_location" {
  default = "northeurope"
}

variable "resource_group_tags" {
  type = "map"

  default = {}
}

locals {
  resource_group_name   = "${var.name_company}-${var.name_platform}-${var.name_project}-rg-gbl-${var.name_environment}"
  cosmosdb_account_name = "${var.name_company}-${var.name_platform}-${var.name_project}-cdb-eun-${var.name_environment}"
}
