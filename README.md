# Demo for OneRail

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
