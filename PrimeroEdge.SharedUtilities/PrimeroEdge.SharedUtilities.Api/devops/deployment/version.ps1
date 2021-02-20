$versionjson = (Get-Content "version.json" -Raw) | ConvertFrom-Json

$version=$versionjson.psobject.properties.value
