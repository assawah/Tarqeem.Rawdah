#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/API/Tarqeem.CA.Rawdah.Web.Api/Tarqeem.CA.Rawdah.Web.Api.csproj", "src/API/Tarqeem.CA.Rawdah.Web.Api/"]
COPY ["src/API/Tarqeem.CA.WebFramework/Tarqeem.CA.WebFramework.csproj", "src/API/Tarqeem.CA.WebFramework/"]
COPY ["src/Core/Tarqeem.CA.Application/Tarqeem.CA.Application.csproj", "src/Core/Tarqeem.CA.Application/"]
COPY ["src/Core/Tarqeem.CA.Domain/Tarqeem.CA.Domain.csproj", "src/Core/Tarqeem.CA.Domain/"]
COPY ["src/Infrastructure/Tarqeem.CA.Infrastructure.CrossCutting/Tarqeem.CA.Infrastructure.CrossCutting.csproj", "src/Infrastructure/Tarqeem.CA.Infrastructure.CrossCutting/"]
COPY ["src/Infrastructure/Tarqeem.CA.Infrastructure.Identity/Tarqeem.CA.Infrastructure.Identity.csproj", "src/Infrastructure/Tarqeem.CA.Infrastructure.Identity/"]
COPY ["src/Infrastructure/Tarqeem.CA.Infrastructure.Persistence/Tarqeem.CA.Infrastructure.Persistence.csproj", "src/Infrastructure/Tarqeem.CA.Infrastructure.Persistence/"]
COPY ["src/Shared/Tarqeem.CA.SharedKernel/Tarqeem.CA.SharedKernel.csproj", "src/Shared/Tarqeem.CA.SharedKernel/"]


RUN dotnet restore "src/API/Tarqeem.CA.Rawdah.Web.Api/Tarqeem.CA.Rawdah.Web.Api.csproj"
COPY . .
WORKDIR "src/API/Tarqeem.CA.Rawdah.Web.Api"
RUN dotnet build "Tarqeem.CA.Rawdah.Web.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Tarqeem.CA.Rawdah.Web.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tarqeem.CA.Rawdah.Web.Api.dll"]
