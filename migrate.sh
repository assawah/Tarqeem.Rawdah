#!/usr/bin/env sh

dotnet ef migrations add $1 --project src/Infrastructure/Tarqeem.CA.Infrastructure.Persistence --startup-project src/API/Tarqeem.CA.Rawdah.Web.Api
