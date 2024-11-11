FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/Sample.Notify.WebApi/Sample.Notify.WebApi.csproj", "src/Sample.Notify.WebApi/"]
COPY ["src/Sample.Notify.WebApi.Core/Sample.Notify.WebApi.Core.csproj", "src/Sample.Notify.WebApi.Core/"]
RUN dotnet restore "src/Sample.Notify.WebApi/Sample.Notify.WebApi.csproj"
COPY . .
WORKDIR "/src/src/Sample.Notify.WebApi"
RUN dotnet build "Sample.Notify.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Sample.Notify.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sample.Notify.WebApi.dll"]