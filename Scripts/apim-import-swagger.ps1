param ($api, $env)

$apiList = @{ accountmanagement = "accountservice.perseusedge.com"; `
              administration = "administrationservice.perseusedge.com"; `
			  usermanagement = "usermanagementservice.perseusedge.com"; `
			  customuserviews = "accountservice.perseusedge.com"; `
			  dataexchangepatron = "eligibilityservice.perseusedge.com" ;
			  eligibility = "eligibilityservice.perseusedge.com"; `
			  inventorymanagement = "inventoryservice.perseusedge.com" ; `
			  itemmanagement = "itemservice.perseusedge.com"; `
			  menuplanning = "menuplanningservice.perseusedge.com"; `
			  pointofservice = "posservice.perseusedge.com"; `
			  dataexchange = "dataexchangeservice.perseusedge.com"; `
			  productionplanning = "productionplanningservice.perseusedge.com"; `
			  recipemanagement = "recipeservice.perseusedge.com"; `
			  sharedutilities = "sharedutilitiesservice.perseusedge.com"; `
			  workspace = "workspaceservice.perseusedge.com"; }


$domain = $apiList[$api]
$apiUrl = "${env}${domain}"

# login to Azure
az login --service-principal `
         --tenant ${env:TENANTID} `
         --username ${env:CLIENTID} `
         --password ${env:CLIENTSECRET}

# default DevTst Subscription
az account set --subscription ${env:SUBSCRIPTIONID}

# publish the api
az apim api import --path $api `
                   --api-id $api `
				   --resource-group "schoolcafe-rgglobal-${env}-eastus2" `
				   --service-name "schoolcafe-apim-${env}-eastus2" `
				   --specification-url "https://${apiUrl}/swagger/v1/swagger.json" `
                   --specification-format "OpenApiJson"
