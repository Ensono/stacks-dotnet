########################################
# Provider to connect to AWS
# https://www.terraform.io/docs/providers/aws/
########################################
provider "aws" {
  region = var.region

  default_tags {
    tags = local.default_tags
  }
}

terraform {
  required_version = ">= 1.9"

  backend "s3" {
    # Configured via runtime command line flags
    encrypt = true
  }

  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = ">= 3.37.0"
    }

    random = {
      source  = "hashicorp/random"
      version = "3.6.3"
    }
  }
}
