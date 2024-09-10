############################################
# Naming
############################################
variable "seed_length" {
  type    = number
  default = 6
}

variable "function_name" {
  type        = string
  description = "Name of the function app"
}

variable "app_service_plan_name" {
  type    = string
  default = "app-sp-events"
}

############################################
# Azure Information
############################################
variable "resource_group_name" {
  type        = string
  description = "Name of hte resource grup holding the resources"
}

variable "resource_group_location" {
  type        = string
  description = "Region in Azure that the resources should be deployed to"
}

############################################
# Function App Settings
############################################
variable "az_function_extension_version" {
  type        = string
  default     = "~4"
  description = "Version of the Azure Function runtime to use"
}

variable "az_function_dotnet_version" {
  type        = string
  default     = "v8.0"
  description = "Version of the .NET framework to use in the function"
}

############################################
# CosmosDB
############################################
variable "cosmosdb_collection_name" {
  type        = string
  description = "Name of the CosmosDB collection to use"
}

variable "cosmosdb_connection_string" {
  type        = string
  description = "Connection string for the CosmosDB"
}

variable "cosmosdb_database_name" {
  type        = string
  description = "Name of the CosmosDB database to use"
}

variable "cosmosdb_lease_collection_name" {
  type        = string
  default     = "Leases"
  description = "Name of the CosmosDB lease collection to use"
}

############################################
# Service Bus
############################################
variable "servicebus_connection_string" {
  type        = string
  description = "Connection string for Service Bus"
}

variable "sb_topic_name" {
  type        = string
  description = "Name of the Service Bus topic"
}

variable "sb_subscription_name" {
  type        = string
  description = "Name of the Service Bus subscription"
}
