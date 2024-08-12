# Required variables

variable "resource_group_name" {
  type        = string
  description = "Name of hte resource grup holding the resources"
}

variable "resource_group_location" {
  type        = string
  description = "Region in Azure that the resources should be deployed to"
}

variable "eh_name" {
  type        = string
  default     = "stacks-event-hub"
  description = "Name of the Event Hub"
}

variable "eh_capacity" {
  type    = number
  default = 1
}

variable "eh_partition_count" {
  type    = number
  default = 2
}

variable "eh_retention" {
  type        = number
  default     = 1
  description = "How many days to retain the events for this Event Hub"
}

variable "eh_blob_container" {
  type    = string
  default = "stacks-blob-container"
}

variable "eventhub_sku" {
  type    = string
  default = "Standard"
}

variable "app-service-plan-name" {
  type    = string
  default = "app-sp-events"
}

variable "func-aeh-listener-name" {
  type    = string
  default = "func-aeh-listener"
}

variable "seed_length" {
  type    = number
  default = 6
}

variable "az_function_extension_version" {
  type        = string
  default     = "~4"
  description = "Version of the Azure Function runtime to use"
}

variable "az_function_dotnet_version" {
  type        = string
  default     = "v6.0"
  description = "Version of the .NET framework to use in the function"
}
