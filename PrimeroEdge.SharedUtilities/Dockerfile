FROM perseusnonprodacr.azurecr.io/dotnet6/dotnet-base-6.0.411 AS  build
WORKDIR /src
COPY NuGet.Config ./
COPY ["PrimeroEdge.SharedUtilities.Api/PrimeroEdge.SharedUtilities.Api.csproj", "PrimeroEdge.SharedUtilities.Api/"]
COPY ["PrimeroEdge.SharedUtilities.Components/PrimeroEdge.SharedUtilities.Components.csproj", "PrimeroEdge.SharedUtilities.Components/"]
RUN dotnet restore --configfile NuGet.Config "PrimeroEdge.SharedUtilities.Api/PrimeroEdge.SharedUtilities.Api.csproj"

# copy remainder artifacs and build
COPY . .
WORKDIR "/src/PrimeroEdge.SharedUtilities.Api"
RUN dotnet publish "PrimeroEdge.SharedUtilities.Api.csproj" -c release -o /app/publish --no-restore

FROM  perseusnonprodacr.azurecr.io/dotnet6/dotnet-slim-6.0.411
WORKDIR /app
EXPOSE 5000

# Create a group and user
RUN groupadd -g 1001 1001 && useradd -u 1001 -g 1001 1001
USER 1001

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "PrimeroEdge.SharedUtilities.Api.dll"]