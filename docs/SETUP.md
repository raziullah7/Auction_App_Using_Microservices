# Local development and checks

[Project overview](../README.md)

## Setup and usage

### Prerequisites

- Git and Docker with the Docker Compose plugin, configured for Linux containers.
- Local DNS/hosts-file access and trusted development certificates for the three hostnames below.
- .NET 8 SDK for running backend tests outside Docker.
- Node.js and npm for optional frontend checks; the current frontend Dockerfile uses Node.js 18.

### Run the Docker environment

```bash
git clone https://github.com/raziullah7/Auction_App_Using_Microservices.git
cd Auction_App_Using_Microservices
```

Add this entry to your operating system's hosts file:

```text
127.0.0.1 app.carsties.local id.carsties.local api.carsties.local
```

Provide a locally trusted certificate and key covering those names at `devcerts/carsties.local.crt` and `devcerts/carsties.local.key`, matching the existing nginx mount. For example, after installing [mkcert](https://github.com/FiloSottile/mkcert), generate your own local certificate from the repository root:

```bash
mkcert -install
mkcert -cert-file devcerts/carsties.local.crt -key-file devcerts/carsties.local.key "*.carsties.local" carsties.local
```

Keep generated private keys local. Review the development configuration in [docker-compose.yml](../docker-compose.yml), including database connection strings, identity URLs, and `AUTH_SECRET`, before starting. The checked-in configuration is for local development.

```bash
docker compose up -d --build
docker compose ps
docker compose logs -f
```

Open `https://app.carsties.local`. Identity uses `https://id.carsties.local`, and the gateway uses `https://api.carsties.local`. Local ports 80/443, database ports, and the service ports declared in Compose must be available. Inspect service logs if dependencies are still starting.

For frontend-only development commands, see the existing [frontend README](../frontend/web-app/README.md). Its development server also requires reachable backend services and matching authentication configuration.

## Existing checks

Run from the repository root:

```bash
dotnet test tests/AuctionService.Unit.Tests/AuctionService.Unit.Tests.csproj
dotnet test tests/AuctionService.IntegrationTests/AuctionService.IntegrationTests.csproj
dotnet test tests/SearchService.IntegrationTests/SearchService.IntegrationTests.csproj
```

- **Auction unit tests:** controller responses, ownership checks, and entity behavior.
- **Auction integration tests:** HTTP responses, authentication/authorization, persistence, and auction-created event publishing. These use PostgreSQL Testcontainers, so Docker must be running.
- **Search integration tests:** auction-created event consumption and MongoDB persistence, using Mongo2Go and the MassTransit test harness. The environment must support Mongo2Go's database process.

Frontend scripts are defined in [package.json](../frontend/web-app/package.json):

```bash
cd frontend/web-app
npm ci
npm run lint
npm run build
```


