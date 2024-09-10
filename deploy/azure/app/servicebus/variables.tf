

# Required variables

variable "resource_group_name" {
  type        = string
  description = "Name of hte resource grup holding the resources"
}

variable "resource_group_location" {
  type        = string
  description = "Region in Azure that the resources should be deployed to"
}

# Optional variables
# These have default values that can be overriden as required
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

variable "sb_subscription_filtered_name" {
  type        = string
  default     = "sbs-menu-events-filtered"
  description = "Name of the Service Bus subscription, filtered, to create"
}

variable "sb_subscription_name" {
  type        = string
  default     = "sbs-menu-events"
  description = "Name of the Service Bus subscription to create"
}

variable "sb_max_delivery_count" {
  type    = number
  default = 1
}

variable "seed_length" {
  type    = number
  default = 6
}

variable "servicebus_sku" {
  type    = string
  default = "Standard"
}
