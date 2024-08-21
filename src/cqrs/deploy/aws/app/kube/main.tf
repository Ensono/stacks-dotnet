
module "app_label" {
  source     = "cloudposse/label/null"
  version    = "0.24.1"
  namespace  = "${var.name_company}-${var.name_project}"
  stage      = var.stage
  name       = "${lookup(var.location_name_map, var.region, "eu-west-2")}-${var.name_domain}"
  attributes = var.attributes
  delimiter  = "-"
  tags       = var.tags
}

module "app" {

  source = "git::https://github.com/amido/stacks-terraform//aws/modules/infrastructure_modules/stacks_app?ref=master"

  enable_dynamodb = var.enable_dynamodb
  table_name      = "${module.app_label.id}-${var.table_name}"
  hash_key        = var.hash_key
  attribute_name  = var.attribute_name
  attribute_type  = var.attribute_type
  enable_queue    = contains(split(",", var.app_bus_type), "sns") ? var.enable_queue : false
  queue_name      = "${module.app_label.id}-${var.queue_name}"
  tags            = module.app_label.tags
}

################
# Image Repositories (TODO: Should be list of strings created in app module)
###############

resource "aws_ecr_repository" "docker_image" {
  count = var.create_docker_repositories ? 1 : 0
  name  = var.docker_image_name
  image_scanning_configuration {
    scan_on_push = false
  }
  # Pass Default Tag Values to Underlying Modules
  tags = var.tags
}

resource "aws_ecr_repository" "docker_image_bg_worker" {
  count = var.create_docker_repositories ? (contains(split(",", var.app_bus_type), "servicebus") || contains(split(",", var.app_bus_type), "eventhub") ? 1 : 0) : 0
  name  = var.docker_image_name_bg_worker
  image_scanning_configuration {
    scan_on_push = false
  }
  # Pass Default Tag Values to Underlying Modules
  tags = var.tags
}

resource "aws_ecr_repository" "docker_image_worker_function" {
  count = var.create_docker_repositories ? (contains(split(",", var.app_bus_type), "servicebus") || contains(split(",", var.app_bus_type), "eventhub") ? 1 : 0) : 0
  name  = var.docker_image_name_worker
  image_scanning_configuration {
    scan_on_push = false
  }
  # Pass Default Tag Values to Underlying Modules
  tags = var.tags
}

resource "aws_ecr_repository" "docker_image_asb_function" {
  count = var.create_docker_repositories ? (contains(split(",", var.app_bus_type), "servicebus") ? 1 : 0) : 0
  name  = var.docker_image_name_asb_listener
  image_scanning_configuration {
    scan_on_push = false
  }
  # Pass Default Tag Values to Underlying Modules
  tags = var.tags
}

resource "aws_ecr_repository" "docker_image_aeh_function" {
  count = var.create_docker_repositories ? (contains(split(",", var.app_bus_type), "eventhub") ? 1 : 0) : 0
  name  = var.docker_image_name_aeh_listener
  image_scanning_configuration {
    scan_on_push = false
  }
  # Pass Default Tag Values to Underlying Modules
  tags = var.tags
}
