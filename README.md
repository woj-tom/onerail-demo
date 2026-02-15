# Demo for OneRail

A distributed microservices demo built with **ASP.NET Core**, demonstrating:

- Clean architecture principles
- JWT-based authentication
- Event-driven communication using MassTransit
- RabbitMQ messaging
- PostgreSQL per service
- Docker-based local development
- Integration testing with Testcontainers

This project is part of a job application and is designed to showcase practical .NET distributed system skills.

---

## Architecture Overview

The solution follows a microservices approach with clear separation of concerns.

### Services

- **Product Service**  
  Responsible for managing product catalog data.

- **Inventory Service**  
  Responsible for tracking and updating product inventory.

- **Shared Contracts**  
  Contains messaging contracts shared between services.

### Communication

- HTTP for external API access
- RabbitMQ (via MassTransit) for inter-service communication
- JWT for securing public APIs

Each service owns its own database.

## Tech Stack

- ASP.NET Core 10
- MassTransit
- RabbitMQ
- PostgreSQL
- Docker
- Docker Compose
- Testcontainers
- xUnit

## Local development

Run environment for a local development
* PostgreSQL for Inventory Service on `localhost:6100`
* PostgreSQL for Product Service on `localhost:6200`
* RabbitMQ with admin panel on default ports

```bash
docker compose -f ./deploy/docker/docker-compose.local.yml up -d
```

Run a whole environment to test communication

```bash
docker compose -f ./deploy/docker/docker-compose.yml up -d --build
```

## Building images

### Build Inventory Service

```bash
docker build -t one-rail/inventory-service -f src/Services/InventoryService/InventoryService.API/Dockerfile . 
```

### Build Product Service

```bash
docker build -t one-rail/product-service -f src/Services/ProductService/ProductService.API/Dockerfile . 
```
