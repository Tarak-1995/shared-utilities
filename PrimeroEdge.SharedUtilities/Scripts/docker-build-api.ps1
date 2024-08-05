# login into acr
Import-Module Az
az login 
az account set --subscription 'b0b91233-8bb9-44e7-b548-84412ecc5c0d'
az acr login --name perseusnonprodacr

docker build --tag primeroedge.sharedutilities.api.dev:1.0 ..
