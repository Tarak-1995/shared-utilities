$version = (Get-Content "version.json" -Raw) | ConvertFrom-Json

$version.psobject.properties.value