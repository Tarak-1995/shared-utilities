FROM mcr.microsoft.com/dotnet/core/aspnet:latest AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/core/sdk:latest AS  build
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
RUN addgroup -g 1001 -S 1001 \
&& adduser -S -u 1001 -G 1001 -s /bin/bash
USER 1001
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PrimeroEdge.SharedUtilities.Api.dll"] 