$versionjson = (Get-Content "PrimeroEdge.SharedUtilities/PrimeroEdge.SharedUtilities.Api/devops/deployment/sharedutilities/version.json" -Raw) | ConvertFrom-Json
$versionfromjson=$versionjson.psobject.properties.value
Write-Host "##vso[task.setvariable variable=Version;]$versionfromjson" 
Write-Host "##vso[task.setvariable variable=helmChartVersion;]$versionfromjson"
#Write-Host "Using version: ($env:Version)"