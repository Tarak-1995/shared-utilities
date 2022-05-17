FROM perseusnonprodacr.azurecr.io/alldockerimages/aspnet AS base
WORKDIR /app
EXPOSE 5000

FROM perseusnonprodacr.azurecr.io/alldockerimages/sdk AS  build
WORKDIR /src
COPY NuGet.Config ./
COPY ["PrimeroEdge.SharedUtilities.Api/PrimeroEdge.SharedUtilities.Api.csproj", "PrimeroEdge.SharedUtilities.Api/"]
COPY ["PrimeroEdge.SharedUtilities.Components/PrimeroEdge.SharedUtilities.Components.csproj", "PrimeroEdge.SharedUtilities.Components/"]
RUN dotnet restore --configfile NuGet.Config "PrimeroEdge.SharedUtilities.Api/PrimeroEdge.SharedUtilities.Api.csproj"
COPY . .
WORKDIR "/src/PrimeroEdge.SharedUtilities.Api"
RUN dotnet build "PrimeroEdge.SharedUtilities.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PrimeroEdge.SharedUtilities.Api.csproj" -c Release -o /app/publish

FROM base AS final
# Create a group and user
RUN groupadd -g 1001 1001 \
&& useradd -u 1001 -g 1001 1001
USER 1001
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PrimeroEdge.SharedUtilities.Api.dll"] 