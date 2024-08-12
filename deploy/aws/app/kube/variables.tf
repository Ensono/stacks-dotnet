variable "docker_image_name" {
  type = string
}

variable "create_docker_repositories" {
  type    = bool
  default = false
}

variable "tags" {
  description = "Meta data for labelling the infrastructure"
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
  description = "Responsible parties"
  type        = string
}

variable "region" {
  description = "AWS Region for this infrastruture"
  type        = string
  default     = "eu-west-2"
}
