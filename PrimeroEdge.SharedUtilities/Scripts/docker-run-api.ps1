docker build .. -t primeroedge.sharedutilities.api.dev:1.0
docker run -d -e ASPNETCORE_ENVIRONMENT='Development' --publish 5000:5000 --name primeroedge.sharedutilities.api primeroedge.sharedutilities.api.dev:1.0
Start-Sleep -s 5
[system.Diagnostics.Process]::Start("chrome","http://localhost:5000:/swagger")