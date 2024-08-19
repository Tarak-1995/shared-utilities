param ($api, $env, $openapifile)

$path = $api
$apiId = "${api}-v1"

# login to Azure
az login --service-principal `
         --tenant ${env:TENANTID} `
         --username ${env:CLIENTID} `
         --password ${env:CLIENTSECRET}

# default DevTst Subscription
az account set --subscription ${env:SUBSCRIPTIONID}

az apim api import --path $path `
                   --api-id $apiId `
                   --api-version "v1.0" `
				   --resource-group "schoolcafe-rgglobal-${env}-eastus2" `
				   --service-name "schoolcafe-apim-${env}-eastus2" `
				   --specification-path $openapifile `
                   --specification-format "OpenApiJson"
