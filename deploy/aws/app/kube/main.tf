resource "aws_ecr_repository" "docker_image" {
  count = var.create_docker_repositories ? 1 : 0
  name  = var.docker_image_name
  image_scanning_configuration {
    scan_on_push = false
  }
  # Pass Default Tag Values to Underlying Modules
  tags = var.tags
}
