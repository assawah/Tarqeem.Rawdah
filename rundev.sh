#!/usr/bin/env sh

ASPNETCORE_URLS=http://localhost:5002 ASPNETCORE_ENVIRONMENT=Development dotnet run --environment Development --project ./src/API/Tarqeem.CA.Rawdah.Web.Api
