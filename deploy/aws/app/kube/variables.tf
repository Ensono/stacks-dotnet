############################################
# NAMING
############################################

variable "name_company" {
  type    = string
  default = "ensono"
}

variable "name_project" {
  type    = string
  default = "stacks"
}

variable "name_component" {
  type    = string
  default = "api"
}

variable "name_environment" {
  type    = string
  default = "nonprod"
}


variable "docker_image_name" {
  type = string
}
variable "create_docker_repositories" {
  type    = bool
  default = false
}

variable "region" {
  description = "AWS Region for this infrastruture"
  type        = string
  default     = "eu-west-2"
}

locals {
  # This is a map of default tags passed to the provider.
  # This can be extended like adding cost-code or organization name.
  default_tags = {
    Environment = var.name_environment
    Component   = var.name_component
    Project     = var.name_project
    Company     = var.name_company
    Region      = var.region
  }
}

locals {
  account_id = data.aws_caller_identity.current.account_id
}
