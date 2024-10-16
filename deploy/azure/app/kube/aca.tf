module "aca" {
  source = "git::https://github.com/Ensono/terraform-azurerm-aca?ref=feature/7427-terraform-module-for-aca"

  resource_group_name = module.default_label.id
  location            = "uksouth"
  resource_tags       = module.default_label.tags

  create_container_app_environment = false
  create_container_app             = true
  create_rg                        = false

  # Container App
  container_app_name = "nginx"
  container_app_registry = {
    server   = "${var.acr_name}.azurecr.io"
    identity = azurerm_user_assigned_identity.default.id
  }
  container_app_identity = {
    type         = "UserAssigned",
    identity_ids = [azurerm_user_assigned_identity.default.id]
  }


  container_app_environment_id        = data.azurerm_container_app_environment.acae.id
  container_app_workload_profile_name = "Consumption"
  container_app_revision_mode         = "Single"

  container_app_containers = [
    {
      cpu    = 0.25
      image  = "nginx:latest"
      memory = "0.5Gi"
      name   = "nginx"
      # env = [{
      #   name  = "API_BASE_URL"
      #   value = "https://backendapp.lemonocean-9d31a840.uksouth.azurecontainerapps.io"
      # }]
      volume_mounts = [
        # {
        #   name = "data"
        #   path = "/app/data"
        # }
      ]
    }

  ]
  container_app_container_max_replicas = 1
  container_app_container_min_replicas = 1
  # container_app_container_volumes      = [
  #   {
  #     name         = "data"
  #     storage_type = "EmptyDir"
  #   }
  # ]

  #  Ingress configuration
  container_app_ingress_external_enabled           = true
  container_app_ingress_target_port                = 80
  container_app_ingress_allow_insecure_connections = true
  create_custom_domain_for_container_app           = true
  custom_domain                                    = "aca.${var.dns_zone}"
}
