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
  type = string
}

variable "public_ip_name" {
  type = string
}

###########################
# CONDITIONAL SETTINGS
##########################
variable "create_resource_group" {
  description = "Whether to create a cosmosdb or not for this application"
  type        = bool
  default     = true
}

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

variable "create_function_app" {
  type        = bool
  default     = false
  description = "Whether to create an Azure Function"
}

variable "app_bus_type" {
  description = "Which app bus to use."
  type        = string
  default     = ""
  validation {
    condition = anytrue([
      contains(split(",", var.app_bus_type), "servicebus"),
      contains(split(",", var.app_bus_type), "eventhub"),
      var.app_bus_type == ""
    ])
    error_message = "The app_bus_type variable must contain servicebus or eventhub."
  }
}

###########################
# CosmosDB Settings
##########################
variable "cosmosdb_account_name" {
  type        = string
  description = "Name of an existing CosmosDB account if not creating a new one"
  default     = ""
  validation {
    condition     = !var.create_cosmosdb && var.create_function_app ? length(var.cosmosdb_account_name) > 0 : true
    error_message = "You must specify a value for cosmosdb_account_name if create_cosmosdb is false and create_function_app is true."
  }
}

variable "cosmosdb_resource_group_name" {
  type        = string
  description = "Name of an existing CosmosDB resource group if not creating a new one"
  default     = ""
  validation {
    condition     = !var.create_cosmosdb && var.create_function_app ? length(var.cosmosdb_resource_group_name) > 0 : true
    error_message = "You must specify a value for cosmosdb_resource_group_name if create_cosmosdb is false and create_function_app is true."
  }
}

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
# Service Bus Settings
##########################
variable "sb_name" {
  type        = string
  default     = "sb-menu"
  description = "Name of the service bus to create"
}

variable "sb_topic_name" {
  type        = string
  default     = "sbt-menu-events"
  description = "Name of the topic to create"
}

variable "sb_subscription_name" {
  type        = string
  default     = "sbs-menu-events"
  description = "Name of the Service Bus subscription to create"
}

variable "sb_subscription_filter" {
  type        = string
  description = "SQL Filter for the Service Bus subscription"
  default     = ""
}

variable "sb_resource_group_name" {
  type        = string
  default     = ""
  description = "Name of the resource group that holds the the above resources"
}

###########################
# Azure Function App
##########################
variable "function_name" {
  type        = string
  description = "Name of the function app"
  default     = ""
}

###########################
# Core infrastructure settings
##########################
variable "core_environment" {
  type        = string
  description = "Name of the environment for the core infrastructure"
  default     = "nonprod"
}

variable "core_resource_group" {
  type        = string
  description = "Name of the resource group for the core infrastructure"
}

variable "tfstate_key" {
  type        = string
  description = "Name of the key in remote storage for the core environmnent"
}

variable "tfstate_storage_account" {
  type        = string
  description = "Name of the storage account that holds the core Terraform state"
}

variable "tfstate_container_name" {
  type        = string
  description = "Name of the container in the specified storage account holding the core state"
}

variable "tfstate_resource_group_name" {
  type        = string
  description = "Name of the resource group that holds the the above resources"
}

##########################
# App Insights Lookup
##########################
variable "app_insights_name" {
  type        = string
  default     = ""
  description = "app insights name for key retriaval in memory"
}

