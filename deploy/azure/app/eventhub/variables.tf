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

variable "seed_length" {
  type    = number
  default = 6
}
