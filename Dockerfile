FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_HTTP_PORTS=8080
ENV ASPNETCORE_URLS=http://+:8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["TransitDesk.sln", "./"]
COPY ["TransitDesk.Core/TransitDesk.Core.csproj", "TransitDesk.Core/"]
COPY ["TransitDesk.Web/TransitDesk.Web.csproj", "TransitDesk.Web/"]
COPY ["TransitDesk.Tests/TransitDesk.Tests.csproj", "TransitDesk.Tests/"]

RUN dotnet restore "TransitDesk.Web/TransitDesk.Web.csproj"

COPY . .
WORKDIR "/src/TransitDesk.Web"
RUN dotnet build "TransitDesk.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TransitDesk.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TransitDesk.Web.dll"]
