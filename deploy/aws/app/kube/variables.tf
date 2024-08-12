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
}

variable "docker_image_name_bg_worker" {
  description = "BG Worker docker image name."
  type        = string
}

variable "docker_image_name_worker" {
  description = "K8S Worker docker image name."
  type        = string
}

variable "docker_image_name_asb_listener" {
  description = "ASB Listener docker image name."
  type        = string
}

variable "docker_image_name_aeh_listener" {
  description = "ASB Listener docker image name."
  type        = string
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

variable "stage" {
  description = "ID element. Usually used to indicate role, e.g. 'prod', 'staging','test', 'deploy', 'release'."
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
  description = "The name of the environment."
  default     = "nonprod"
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
  nullable    = true
  validation {
    condition = anytrue([
      var.app_bus_type == "servicebus",
      var.app_bus_type == "eventhub",
      var.app_bus_type == "sns",
      var.app_bus_type == null
    ])
    error_message = "The app_bus_type variable must be null, servicebus, eventhub, or sns."
  }
}

variable "enable_queue" {
  description = "Whether to create SQS queue and SNS topic. Must match app_bus_type above."
  type        = bool
}

variable "queue_name" {
  description = "This is the human-readable name of the queue and topic. If omitted, Terraform will assign a random name."
  type        = string
}

##################################################
# Dynamo DB
##################################################
variable "enable_dynamodb" {
  description = "Whether to create dynamodb table."
  type        = bool
}

variable "table_name" {
  description = "The name of the table, this needs to be unique within a region."
  type        = string
}

variable "hash_key" {
  description = "The attribute to use as the hash (partition) key."
  type        = string
}

variable "attribute_name" {
  description = "Name of the attribute."
  type        = string
}

variable "attribute_type" {
  description = "Type of the attribute, which must be a scalar type: S, N, or B for (S)tring, (N)umber or (B)inary data."
  type        = string
}
