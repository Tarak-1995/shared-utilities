﻿$versionjson = (Get-Content "PrimeroEdge.SharedUtilities/PrimeroEdge.SharedUtilities.Api/devops/deployment/sharedutilities/version.json" -Raw) | ConvertFrom-Json

$version=$versionjson.psobject.properties.value
