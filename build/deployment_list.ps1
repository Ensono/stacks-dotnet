@(
    @{
        displayName = "AppDeployment"
        template = "deploy/k8s/app/base_api-deploy.yml"
        vars = @{
            dns_pointer = "`${ENV_NAME}-`${DOMAIN}.`${BASE_DOMAIN}"
            tls_domain = "`${BASE_DOMAIN}"
            ks8_app_path = "/api/menu"
            log_level = "Debug"
            k8s_image = "`${DOCKER_REGISTRY}/`${DOCKER_IMAGE_NAME}:`${DOCKER_IMAGE_TAG}"
            aadpodidentitybinding = "stacks-webapp-identity"
            app_insights_key = "`${APP_INSIGHTS_INSTRUMENTATION_KEY}"
            jwtbearerauthentication_audience = "<TODO>"
            jwtbearerauthentication_authority = "<TODO>"
            jwtbearerauthentication_enabled = false
            jwtbearerauthentication_openapiauthorizationurl = "<TODO>"
            jwtbearerauthentication_openapiclientid = "<TODO>"
            jwtbearerauthentication_openapitokenurl = "<TODO>"
        }
    }
)