############################################
# AUTHENTICATION
############################################

# Azure SPN credentials used by terraform
client_id = "#{azure_client_id}"
client_secret = "#{azure_client_secret}"
subscription_id = "#{azure_subscription_id}"
tenant_id = "#{azure_tenant_id}"

############################################
# NAMING
############################################
name_company = "#{name_company}"
name_environment = "#{name_environment}"
name_platform = "#{name_platform}"
name_project = "#{name_project}"

############################################
# RESOURCE GROUP INFORMATION
############################################
resource_group_location = "#{resource_group_location}"
resource_group_tags = {}

############################################
# DNS
############################################

cluster_ingress_address = {
    eun = "ingress-amido-ref-cluster-eun-dev.k8s.dbw.cloud"
    euw = "ingress-amido-ref-cluster-eun-dev.k8s.dbw.cloud"
}