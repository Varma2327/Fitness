# ----- Build stage -----
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app/out

# ----- Runtime stage -----
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# place for SQLite if you use it
RUN mkdir -p /data

# Render sets PORT; bind to it
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT}

# If you want to force SQLite path via env:
# ENV ConnectionStrings__Default="Data Source=/data/planetgym.db"

COPY --from=build /app/out ./

# optional: mark /data as a volume
VOLUME ["/data"]

ENTRYPOINT ["dotnet", "Gym.Api.dll"]
