############################################
# AUTHENTICATION
############################################
# RELYING PURELY ON ENVIRONMENT VARIABLES as the user can control these from their own environment
############################################
# NAMING
############################################

variable "name_company" {
  type = string
}

variable "name_project" {
  type = string
}

variable "name_component" {
  type = string
}

variable "name_domain" {
  type = string
}

variable "stage" {
  type = string
}

variable "attributes" {
  default = []
}

variable "tags" {
  type    = map(string)
  default = {}
}

# Each region must have corresponding a shortend name for resource naming purposes
variable "location_name_map" {
  type = map(string)

  default = {
    northeurope   = "eun"
    westeurope    = "euw"
    uksouth       = "uks"
    ukwest        = "ukw"
    eastus        = "use"
    eastus2       = "use2"
    westus        = "usw"
    eastasia      = "ase"
    southeastasia = "asse"
  }
}

############################################
# AZURE INFORMATION
############################################
variable "resource_group_location" {
  type = string
}

variable "dns_record" {
  type = string
}

###########################
# Core infrastructure settings
##########################
variable "env" {
  type        = string
  description = "name of the deployment environment (i.e. dev/prod)"
  default     = "dev"
}

variable "core_environment" {
  type        = string
  description = "Name of the environment for the core infrastructure"
  default     = "nonprod"
}

variable "core_resource_group" {
  type        = string
  description = "Name of the resource group for the core infrastructure"
}

variable "tfstate_key" {
  type        = string
  description = "Name of the key in remote storage for the core environmnent"
}

variable "tfstate_storage_account" {
  type        = string
  description = "Name of the storage account that holds the core Terraform state"
}

variable "tfstate_container_name" {
  type        = string
  description = "Name of the container in the specified storage account holding the core state"
}

variable "tfstate_resource_group_name" {
  type        = string
  description = "Name of the resource group that holds the the above resources"
}

###########################
# App infrastructure settings
##########################
variable "app_environment" {
  type        = string
  description = "Name of the environment for the app infrastructure. This is the function apps, cosmosdb, etc e.g. kube folder"
  default     = "nonprod"
}

variable "tfstate_app_key" {
  type        = string
  description = "Name of the key in remote storage for the kube environmnent. This is the function apps, cosmosdb, etc e.g. kube folder"
}

##########################
# Azure Container Apps
##########################
variable "deploy_to_aca" {
  type        = bool
  default     = false
  description = "Whether the app is deployed to Azure Container App"
}

variable "docker_image_name" {
  type        = string
  default     = ""
  description = "The name of the image to deploy to ACA"
}

variable "docker_image_tag" {
  type        = string
  default     = ""
  description = "The tag of the image to deploy to ACA"
}

variable "ingress_enabled" {
  type        = bool
  default     = true
  description = "Whether to enabled ingress or not"
}

variable "ingress_port" {
  default     = 8080
  description = "The target port for the ingress"
}

variable "app_route" {
  description = "The API app route"
}

variable "log_level" {
  description = "The application log level"
  default     = "Debug"
}

variable "servicebus_type" {
  default     = ""
  description = "The type of service bus"
}
