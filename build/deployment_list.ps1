@(
    @{
        displayName = "AppDeployment"
        template = "deploy/k8s/app/base_api-deploy.yml"
        vars = @{
            dns_pointer = "`${ENV_NAME}-`${DOMAIN}.`${DNS_BASE_DOMAIN}"
            environment = "`${ENV_NAME}"
            tls_domain = "`${DNS_BASE_DOMAIN}"
            k8s_app_route = "`${K8S_APP_ROUTE}"
            log_level = "Debug"
            k8s_image = "`${DOCKER_REGISTRY}/`${DOCKER_IMAGE_NAME}:`${DOCKER_IMAGE_TAG}"
            aadpodidentitybinding = "stacks-webapp-identity"
            app_insights_key = "`${APP_INSIGHTS_INSTRUMENTATION_KEY}"
            version = "`${DOCKER_IMAGE_TAG}"
            jwtbearerauthentication_audience = "<TODO>"
            jwtbearerauthentication_authority = "<TODO>"
            jwtbearerauthentication_enabled = false # Temporarily disabled directly in template file
            jwtbearerauthentication_openapiauthorizationurl = "<TODO>"
            jwtbearerauthentication_openapiclientid = "<TODO>"
            jwtbearerauthentication_openapitokenurl = "<TODO>"
            rewrite_target = '/$([char]0x0024)2' # Using UniCode to prevent substitution
        }
    }

    @{
      displayName = "AwsDeployment"
      template = "deploy/k8s/aws/base_api-deploy.yml"
      vars = @{
          dns_pointer = "`${ENV_NAME}-`${DOMAIN}.`${DNS_BASE_DOMAIN}"
          environment = "`${ENV_NAME}"
          tls_domain = "`${DNS_BASE_DOMAIN}"
          k8s_app_route = "`${K8S_APP_ROUTE}"
          log_level = "Debug"
          k8s_image = "`${DOCKER_REGISTRY}/`${DOCKER_IMAGE_NAME}:`${DOCKER_IMAGE_TAG}"
          cloudwatch_log_group_name = "`${CLOUDWATCH_LOG_GROUP}"
          cloudwatch_log_prefix ="`${CLOUDWATCH_STREAM_PREFIX}"
          cloudwatch_region = "`${REGION}"
          version = "`${DOCKER_IMAGE_TAG}"
          rewrite_target = '/$([char]0x0024)2' # Using UniCode to prevent substitution
      }
  }
)

# # AWS is expecting env vars or TF outputs for:
# ENV_NAME (i.e. nonprod/prod)
# DOMAIN (i.e. the business domain prefix in DNS)
# DNS_BASE_DOMAIN (i.e. the base DNS domain)
# K8S_APP_ROUTE (i.e. the route after the domain, to the particular service)
# DOCKER_REGISTRY, DOCKER_IMAGE_NAME and DOCKER_IMAGE_TAG to construct the image being used
# CLOUDWATCH_LOG_GROUP, CLOUDWATCH_STREAM_PREFIX and CLOUDWATCH_REGION for logging
# VERSION (for K8S resource versioning)
# TBC: additional vars used in the file which map straight to env-vars
