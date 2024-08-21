############
# Dynamo DB
############
output "dynamodb_table_arn" {
  description = "ARN of the DynamoDB table"
  value       = var.enable_dynamodb ? module.app.dynamodb_table_arn[0] : null
}

output "dynamodb_table_id" {
  description = "ID of the DynamoDB table"
  value       = var.enable_dynamodb ? module.app.dynamodb_table_id[0] : null
}

# TODO: This needs to be an output from the app module, rather than being constructed
output "dynamodb_table_name" {
  description = "Name of the DynamoDB table (constructed)"
  value       = "${module.app_label.id}-${var.table_name}"
}

############
# SQS
############
output "sqs_queue_id" {
  description = "The URL for the created Amazon SQS queue"
  value       = var.enable_queue ? module.app.sqs_queue_id : null
}

output "sqs_queue_arn" {
  description = "The ARN of the SQS queue"
  value       = var.enable_queue ? module.app.sqs_queue_arn : null
}

############
# SNS TODO: Tactical, needs to be incorporated into app module
############
output "sns_topic_arn" {
  description = "The ARN for the created Amazon SNS topic"
  value       = var.enable_queue ? module.app.sns_topic_arn : null
}

