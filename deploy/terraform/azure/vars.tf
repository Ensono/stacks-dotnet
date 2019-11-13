############################################
# AUTHENTICATION
############################################
############################################
# NAMING
############################################

variable "name_company" {
}

variable "name_environment" {
}

variable "name_platform" {
}

variable "name_component" {
  
}

# Each region must have corresponding a shortend name for resource naming purposes 
variable "location_name_map" {
  type = map(string)

  default = {
    northeurope   = "eun"
    westeurope    = "euw"
    uksouth       = "uks"
    ukwest        = "ukw"
    eastus        = "use"
    eastus2       = "use2"
    westus        = "usw"
    eastasia      = "ase"
    southeastasia = "asse"
  }
}

############################################
# RESOURCE GROUP INFORMATION
############################################

variable "resource_group_location_env" {
  default = "uksouth"
}

variable "resource_group_tags" {
  type = map(string)

  default = {}
}

############################################
# COSMOSDB INFORMATION
############################################

variable "cosmosDBdatabaseName" {
}
###########################

locals {
  cosmosdb_account_name            = "${var.name_company}${var.name_platform}${var.name_component}cda${var.location_name_map[var.resource_group_location_env]}${var.name_environment}"
  cosmosdb_table_name             = "${var.name_company}${var.name_platform}${var.name_component}cdt${var.location_name_map[var.resource_group_location_env]}${var.name_environment}"
  cosmosdb_db_name             = "${var.name_company}${var.name_platform}${var.name_component}cdb${var.location_name_map[var.resource_group_location_env]}${var.name_environment}"
  resource_group_name_env      = "${var.name_company}-${var.name_platform}-${var.name_component}-rg-${var.location_name_map[var.resource_group_location_env]}-${var.name_environment}"

}

