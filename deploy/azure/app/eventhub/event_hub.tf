
# create a random string to be used to create
# similar but different resources
resource "random_string" "seed" {
  length  = var.seed_length
  upper   = false
  number  = false
  special = false
}

resource "azurerm_eventhub_namespace" "eh_ns" {
  name                = "${var.eh_name}-${random_string.seed.result}"
  resource_group_name = var.resource_group_name
  location            = var.resource_group_location
  sku                 = var.eventhub_sku
  capacity            = var.eh_capacity
}

resource "azurerm_eventhub" "eh" {
  name                = var.eh_name
  namespace_name      = azurerm_eventhub_namespace.eh_ns.name
  resource_group_name = var.resource_group_name
  partition_count     = var.eh_partition_count
  message_retention   = var.eh_retention
  depends_on = [
    azurerm_storage_container.eh_storage_container
  ]

  capture_description {
    enabled  = true
    encoding = "Avro"
    destination {
      name                = "EventHubArchive.AzureBlockBlob"
      archive_name_format = "{Namespace}/{EventHub}/{PartitionId}/{Year}/{Month}/{Day}/{Hour}/{Minute}/{Second}"
      blob_container_name = azurerm_storage_container.eh_storage_container.name
      storage_account_id  = azurerm_storage_account.eh_storage.id
    }
  }
}
