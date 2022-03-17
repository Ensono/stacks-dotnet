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
            rewrite_target = '/$2' # Defined due to variable expansion during `Invoke-Templater`
            2 = '/$2' # Defined due to variable expansion during `Invoke-Templater`
        }
    }
)
