dotnet pack ..\PrimeroEdge.SharedUtilities.Components\PrimeroEdge.SharedUtilities.Components.csproj
nuget push -Source "PrimeroEdgeDev" -ApiKey az -SkipDuplicate ..\PrimeroEdge.SharedUtilities.Components\bin\Debug\PrimeroEdge.SharedUtilities.Components.2.1.10901.nupkg

