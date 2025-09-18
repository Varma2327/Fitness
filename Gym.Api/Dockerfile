# ----- Build stage -----
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app/out

# ----- Runtime stage -----
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Create a writable folder for SQLite file inside the container
RUN mkdir -p /data
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT}
# If you want to force SQLite path in prod, keep this env var on Render too:
# ENV ConnectionStrings__Default="Data Source=/data/planetgym.db"

COPY --from=build /app/out ./

# Ensure the data folder exists at runtime
VOLUME ["/data"]

# NOTE: Render sets $PORT automatically
ENTRYPOINT ["dotnet", "Gym.Api.dll"]
