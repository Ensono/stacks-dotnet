############################################
# Image Repositories
############################################

variable "create_docker_repositories" {
  type    = bool
  default = false
}

variable "docker_image_name" {
  description = "Main docker image."
  type        = string
  default     = ""
  validation {
    condition     = var.create_docker_repositories == true ? length(var.docker_image_name) > 0 : true
    error_message = "You must specify a value for docker_image_name if create_docker_repositories is true."
  }
}

variable "docker_image_name_bg_worker" {
  description = "BG Worker docker image name."
  type        = string
  default     = ""
  validation {
    condition     = var.create_docker_repositories == true && (var.app_bus_type == "servicebus" || var.app_bus_type == "eventhub") ? length(var.docker_image_name_bg_worker) > 0 : true
    error_message = "You must specify a value for docker_image_name_bg_worker if create_docker_repositories is true and app_bus_type is either servicebus or eventhub."
  }
}

variable "docker_image_name_worker" {
  description = "K8S Worker docker image name."
  type        = string
  default     = ""
  validation {
    condition     = var.create_docker_repositories == true && (var.app_bus_type == "servicebus" || var.app_bus_type == "eventhub") ? length(var.docker_image_name_worker) > 0 : true
    error_message = "You must specify a value for docker_image_name_worker if create_docker_repositories is true and app_bus_type is either servicebus or eventhub."
  }
}

variable "docker_image_name_asb_listener" {
  description = "ASB Listener docker image name."
  type        = string
  default     = ""
  validation {
    condition     = var.create_docker_repositories == true && var.app_bus_type == "servicebus" ? length(var.docker_image_name_asb_listener) > 0 : true
    error_message = "You must specify a value for docker_image_name_asb_listener if create_docker_repositories is true and app_bus_type is servicebus."
  }
}

variable "docker_image_name_aeh_listener" {
  description = "ASB Listener docker image name."
  type        = string
  default     = ""
  validation {
    condition     = var.create_docker_repositories == true && var.app_bus_type == "eventhub" ? length(var.docker_image_name_aeh_listener) > 0 : true
    error_message = "You must specify a value for docker_image_name_aeh_listener if create_docker_repositories is true and app_bus_type is eventhub."
  }
}

############################################
# NAMING
############################################
variable "name_company" {
  description = "ID element. Usually used to indicate specific organisation."
  type        = string
}

variable "name_project" {
  description = "ID element. Usually used to indicate specific project."
  type        = string
}

variable "name_domain" {
  description = "ID element. Usually used to indicate specific API."
  type        = string
}

variable "attributes" {
  description = "ID element. List of attributes."
  default     = []
}

variable "tags" {
  description = "Meta data for labelling the infrastructure."
  type        = map(string)
  default     = {}
}

variable "env" {
  description = "The name of the environment, e.g. NonProd, Prod"
  default     = "nonprod"
  type        = string
}

variable "stage" {
  description = "The name of the Stage, e.g. Dev, Test, Prod."
  default     = "dev"
  type        = string
}

variable "owner" {
  description = "Responsible parties for this component."
  type        = string
}

variable "region" {
  description = "AWS Region for this infrastructure."
  type        = string
  default     = "eu-west-2"
}

# Each region must have corresponding a shortend name for resource naming purposes
variable "location_name_map" {

  description = "Each region must have corresponding a shortend name for resource naming purposes."
  type        = map(string)
  default = {

    us-east-1      = "use1"
    us-east-2      = "use2"
    us-west-1      = "usw1"
    us-west-2      = "usw2"
    eu-west-1      = "euw1"
    eu-west-2      = "euw2"
    eu-west-3      = "euw3"
    eu-south-1     = "eus1"
    eu-central-1   = "euc1"
    eu-north-1     = "eun1"
    ap-southeast-1 = "apse1"
    ap-northeast-1 = "apne1"
    ap-southeast-2 = "apse2"
    ap-northeast-2 = "apne2"
    sa-east-1      = "sae1"
    cn-north-1     = "cnn1"
    ap-south-1     = "aps1"
  }
}

###############################################
# Messaging
###############################################

variable "app_bus_type" {
  description = "Which app bus to use."
  type        = string
  default     = ""
  validation {
    condition = anytrue([
      var.app_bus_type == "servicebus",
      var.app_bus_type == "eventhub",
      var.app_bus_type == "sns",
      var.app_bus_type == ""
    ])
    error_message = "The app_bus_type variable must be servicebus, eventhub, or sns."
  }
}

variable "enable_queue" {
  description = "Whether to create SQS queue and SNS topic. Must match app_bus_type above."
  type        = bool
  default     = false
}

variable "queue_name" {
  description = "This is the human-readable name of the queue and topic."
  type        = string
  default     = ""
  validation {
    condition     = var.enable_queue == true ? length(var.queue_name) > 0 : true
    error_message = "You must specify a value for queue_name if enable_queue is true."
  }
}

##################################################
# Dynamo DB
##################################################
variable "enable_dynamodb" {
  description = "Whether to create dynamodb table."
  type        = bool
  default     = false
}

variable "table_name" {
  description = "The name of the table, this needs to be unique within a region."
  type        = string
  default     = ""
  validation {
    condition     = var.enable_dynamodb == true ? length(var.table_name) > 0 : true
    error_message = "You must specify a value for table_name if enable_dynamodb is true."
  }
}

variable "hash_key" {
  description = "The attribute to use as the hash (partition) key."
  type        = string
  default     = ""
  validation {
    condition     = var.enable_dynamodb == true ? length(var.hash_key) > 0 : true
    error_message = "You must specify a value for hash_key if enable_dynamodb is true."
  }
}

variable "attribute_name" {
  description = "Name of the attribute."
  type        = string
  default     = ""
  validation {
    condition     = var.enable_dynamodb == true ? length(var.attribute_name) > 0 : true
    error_message = "You must specify a value for attribute_name if enable_dynamodb is true."
  }
}

variable "attribute_type" {
  description = "Type of the attribute, which must be a scalar type: S, N, or B for (S)tring, (N)umber or (B)inary data."
  type        = string
  default     = ""
  validation {
    condition     = var.enable_dynamodb == true ? length(var.attribute_type) > 0 : true
    error_message = "You must specify a value for attribute_type if enable_dynamodb is true."
  }
}
