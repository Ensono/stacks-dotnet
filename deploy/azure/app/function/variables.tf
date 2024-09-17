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
  description = "Name of the resource group holding the resources"
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
  default     = "8.0"
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
  sensitive   = true
}

variable "cosmosdb_database_name" {
  type        = string
  description = "Name of the CosmosDB database to use"
}

variable "cosmosdb_lease_collection_name" {
  type        = string
  description = "Name of the CosmosDB lease collection to use"
}

############################################
# Service Bus
############################################
variable "sb_connection_string" {
  type        = string
  description = "Connection string for Service Bus"
  sensitive   = true
}

variable "sb_topic_id" {
  type        = string
  description = "ID of the Service Bus topic"
}

variable "sb_topic_name" {
  type        = string
  description = "Name of the Service Bus topic"
}

variable "sb_subscription_name" {
  type        = string
  description = "Name of the Service Bus subscription"
}

variable "sb_max_delivery_count" {
  type    = number
  default = 1
}

variable "sb_subscription_filter" {
  type        = string
  description = "SQL Filter for the Service Bus subscription"
}

############################################
# Event Hub
############################################
variable "eventhub_connection_string" {
  type        = string
  description = "Connection string for the Event Hub"
  sensitive   = true
}

variable "eventhub_name" {
  type        = string
  description = "Name of the Event Hub"
}

variable "eventhub_storage_access_key" {
  type        = string
  description = "Access key for the storage account"
  sensitive   = true
}

variable "eventhub_blob_container" {
  type        = string
  description = "Name of the blob container"
}
