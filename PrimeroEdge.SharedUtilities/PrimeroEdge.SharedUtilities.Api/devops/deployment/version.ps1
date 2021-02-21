$versionjson = (Get-Content "PrimeroEdge.SharedUtilities/PrimeroEdge.SharedUtilities.Api/devops/deployment/sharedutilities/version.json" -Raw) | ConvertFrom-Json
$env:Version=$versionjson.psobject.properties.value
#Write-Host "##vso[task.setvariable variable=Version;]$version" 
Write-Host "Using version: ($env:Version)"