## How to run the project

Run command for build projectt
```Powershell
dotnet build
```
Go to folder contain file `docker-compose`

1. Using docker-compose
```Powershell
docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d --remove-orphans
```

## Application URLs - LOCAL Environment (Docker Container):
- Product API: http://localhost:6002/api/products
- Customer API: http://localhost:6003/api/customers
- Basket API: http://localhost:6004/api/baskets
- Ordering API: http://localhost:6005/api/order
- Inventory API: http://localhost:6006/api/inventory
- Inventory gRPC: http://localhost:6007/api/inventory
- Schedule Job: http://localhost:6008/api/scheduled-jobs
- Payment API: http://localhost:6009/api/payment

2. Using Visual Studio 2022
- Run Compound to start multi projects
---
## Application URLs - DEVELOPMENT Environment:
- Product API: http://localhost:5002/api/products
- Customer API: http://localhost:5003/api/customers
- Basket API: http://localhost:5004/api/baskets
- Ordering API: http://localhost:5005/api/order
- Inventory API: http://localhost:5006/api/inventory
- Inventory gRPC: http://localhost:5007/api/inventory
- Schedule Job: http://localhost:5008/api/scheduled-jobs
- Payment API: http://localhost:5009/api/payment
---
## Application URLs - PRODUCTION Environment:

---
## Packages References

## Install Environment

- https://dotnet.microsoft.com/download/dotnet/6.0
- https://visualstudio.microsoft.com/

## References URLS

## Docker Commands: (cd into folder contain file `docker-compose.yml`, `docker-compose.override.yml`)

- Up & running:
```Powershell
docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d --remove-orphans --build
```
- Stop & Removing:
```Powershell
docker-compose down
```

## Useful commands:

- ASPNETCORE_ENVIRONMENT=Production dotnet ef database update
- dotnet watch run --environment "Development"
- dotnet restore
- dotnet build
- Migration commands for Ordering API:
  - cd into Ordering folder
  - dotnet ef migrations add "SampleMigration" -p Ordering.Infrastructure --startup-project Ordering.API --output-dir Persistence/Migrations
  - dotnet ef migrations remove -p Ordering.Infrastructure --startup-project Ordering.API
  - dotnet ef database update -p Ordering.Infrastructure --startup-project Ordering.API
  - dotnet ef database drop
