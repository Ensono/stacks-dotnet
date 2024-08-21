############################################
# AUTHENTICATION
############################################
# RELYING PURELY ON ENVIRONMENT VARIABLES as the user can control these from their own environment
############################################
# NAMING
############################################

variable "name_company" {
  type = string
}

variable "name_project" {
  type = string
}

variable "name_component" {
  type = string
}

variable "name_domain" {
  type = string
}

variable "stage" {
  type = string
}

variable "attributes" {
  default = []
}

variable "tags" {
  type    = map(string)
  default = {}
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
# AZURE INFORMATION
############################################

variable "resource_group_location" {
  type = string
}

variable "dns_record" {
  description = ""
  type        = string
}

###########################
# CONDITIONAL SETTINGS
##########################
variable "create_cosmosdb" {
  description = "Whether to create a cosmosdb or not for this application"
  type        = bool
}

variable "create_cache" {
  type        = bool
  description = "Whether to create a RedisCache"
}

variable "create_dns_record" {
  type = bool
}

variable "create_cdn_endpoint" {
  type = bool
}

variable "app_bus_type" {
  type    = string
  default = "servicebus"
}

###########################
# CosmosDB SETTINGS
##########################
variable "cosmosdb_sql_container" {
  type        = string
  description = "Specify the SQLContainer name in CosmosDB"
}

variable "cosmosdb_sql_container_partition_key" {
  type        = string
  description = "Specify partition key"
}

variable "cosmosdb_kind" {
  type        = string
  description = "Specify the CosmosDB kind"
}
variable "cosmosdb_offer_type" {
  type        = string
  description = "Specify the offer type"
}

###########################
# Core infrastructure settings
##########################
variable "core_environment" {
  type        = string
  description = "Name of the environment for the core infrastructure"
  default     = "nonprod"
}

variable "tfstate_key" {
  type        = string
  description = "Name of the key in remote storage for the core environmnent"
  default     = "core-sharedservicesenv"
}

variable "tfstate_storage_account" {
  type        = string
  description = "Name of the storage account that holds the Terraform state"
  default     = "amidostackstfstate"
}

variable "tfstate_container_name" {
  type        = string
  description = "Name of the container in the specified storage account holding the state"
  default     = "tfstate"
}

variable "tfstate_resource_group_name" {
  type        = string
  description = "Name of the resource group that holds the the above resources"
  default     = "Stacks-Ancillary-Resources"
}

##########################
# App Insights Lookup
##########################
variable "app_insights_name" {
  type        = string
  default     = ""
  description = "app insights name for key retriaval in memory"
}

