

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
variable "create_sb_namespace" {
  type        = bool
  description = "Create a service bus namespace"
  default     = true
}

variable "create_sb_topic" {
  type        = bool
  description = "Create a service bus topic"
  default     = true
}

variable "create_sb_subscription" {
  type        = bool
  description = "Create a service bus subscription"
  default     = false
}

variable "sb_name" {
  type        = string
  default     = "sb-menu"
  description = "Name of the service bus to create"
  nullable    = false
}

variable "sb_namespace_id" {
  type        = string
  description = "ID of the Service Bus namespace"
  default     = null

  validation {
    condition = anytrue([
      !var.create_sb_topic,
      alltrue([
        var.create_sb_namespace,
        var.create_sb_topic
      ]),
      alltrue([
        !var.create_sb_namespace,
        var.create_sb_topic,
        var.sb_namespace_id != null
      ])
    ])
    error_message = "sb_namespace_id must be set if create_sb_namespace is false and create_sb_topic is true"
  }

}

variable "sb_topic_id" {
  type        = string
  description = "ID of the Service Bus topic"
  default     = null

  validation {
    condition = anytrue([
      !var.create_sb_subscription,
      alltrue([
        var.create_sb_topic,
        var.create_sb_subscription
      ]),
      alltrue([
        !var.create_sb_topic,
        var.create_sb_subscription,
        var.sb_topic_id != null
      ])
    ])
    error_message = "sb_topic_id must be set if create_sb_topic is false and create_sb_subscription is true"
  }
}

variable "sb_topic_name" {
  type        = string
  default     = "sbt-menu-events"
  description = "Name of the topic to create"
  nullable    = false
}

variable "sb_subscription_name" {
  type        = string
  default     = "sbs-menu-events"
  description = "Name of the Service Bus subscription to create"
  nullable    = false
}

variable "sb_subscription_filter" {
  type        = string
  description = "SQL Filter for the Service Bus subscription"
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
