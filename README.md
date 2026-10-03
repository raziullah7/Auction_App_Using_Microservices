# DriveBid

A portfolio vehicle-auction application using **.NET 8 microservices and Next.js**. It supports auction listings, search, identity, bidding checks, live notifications, and background auction completion.

## Stack and structure

- **Services:** auction, bidding, search, identity, notifications, and a YARP gateway.
- **Data:** PostgreSQL, MongoDB, and Entity Framework Core.
- **Communication:** RabbitMQ/MassTransit events, gRPC lookups, and SignalR updates.
- **Frontend:** Next.js and TypeScript; Docker Compose provides the local environment.

The solution and local domains retain the original **Carsties** naming. This is a learning/portfolio implementation associated with a .NET/Next.js microservices course; the repository should not be read as a claim of wholly original architecture.

## Run locally

Follow the [setup and testing guide](docs/SETUP.md). It includes hosts-file entries, local development certificates, environment requirements, and the existing check commands.

```bash
docker compose up -d --build
docker compose ps
```

Run these only after completing the configuration steps. Local certificates and identity URLs are required.

## Scope

Selected auction unit/integration and search integration tests exist. They were not rerun for this documentation update. Event-driven views update asynchronously; the auction service has an outbox, while bid persistence and event publication are separate operations.

Production deployment, full end-to-end coverage, and performance guarantees are not established by this overview.

[Services](src) · [Frontend](frontend/web-app) · [Tests](tests)

