#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Modules/OpenCRM.Manager/OpenCRM.Manager.csproj", "Modules/OpenCRM.Manager/"]
COPY ["Core/OpenCRM.Core.Web/OpenCRM.Core.Web.csproj", "Core/OpenCRM.Core.Web/"]
COPY ["Core/OpenCRM.Core/OpenCRM.Core.csproj", "Core/OpenCRM.Core/"]
COPY ["Modules/OpenCRM.Finance/OpenCRM.Finance.csproj", "Modules/OpenCRM.Finance/"]
COPY ["Modules/OpenCRM.SwissLPD/OpenCRM.SwissLPD.csproj", "Modules/OpenCRM.SwissLPD/"]
RUN dotnet restore "Modules/OpenCRM.Manager/OpenCRM.Manager.csproj"
COPY . .
WORKDIR "/src/Modules/OpenCRM.Manager"
RUN dotnet build "OpenCRM.Manager.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OpenCRM.Manager.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OpenCRM.Manager.dll"]
