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

# provider "kubernetes" {

#   host                     = module.amido_stacks_infra.cluster_endpoint
#   cluster_ca_certificate   = base64decode(module.amido_stacks_infra.cluster_certificate_authority_data)
#   config_context_auth_info = "aws"

#   exec {
#     api_version = "client.authentication.k8s.io/v1alpha1"
#     command     = "aws"
#     args        = ["eks", "get-token", "--cluster-name", module.amido_stacks_infra.cluster_name]
#   }

# }

terraform {
  required_version = ">= 0.14"

  backend "s3" {
    bucket         = "s3-ew2-terraform-eks-infra-dev-terraform-backend-24"
    region         = "eu-west-2"
    key            = "terraform-dotnet/eu-west-2/dev/dotnet.tfstate"
    dynamodb_table = "dynamo-ew2-terraform-dotnet-infra-dev-terraform-state-lock"
    encrypt        = true
  }

  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = ">= 3.20.0"
    }

    random = {
      source  = "hashicorp/random"
      version = "3.1.2"
    }
  }
}
