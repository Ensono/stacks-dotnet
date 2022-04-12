resource "random_string" "random" {
  length           = 16
  special          = true
  override_special = "/@£$"
}

output "random" {
  description = "Output random string"
  value       = random_string.random.result
}
