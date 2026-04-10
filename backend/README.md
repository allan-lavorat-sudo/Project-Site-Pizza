# Backend - Pizza Delivery API

API robusta em ASP.NET Core para gerenciar o site de pizzaria.

## Estrutura

```
backend/
├── PizzaDelivery.API/           # Projeto principal
│   ├── Controllers/             # Endpoints da API
│   ├── Models/                  # Modelos de dados
│   ├── DTOs/                    # Data Transfer Objects
│   ├── Services/                # Lógica de negócio
│   ├── Repositories/            # Acesso a dados
│   ├── Middleware/              # Custom middleware
│   ├── Configuration/           # Setup de DI
│   └── appsettings.json
├── PizzaDelivery.Data/          # Entity Framework
│   ├── ApplicationDbContext.cs
│   ├── Entities/
│   └── Migrations/
├── PizzaDelivery.Domain/        # Business logic
│   ├── Entities/
│   ├── Services/
│   └── Interfaces/
├── PizzaDelivery.Tests/         # Testes
└── docker-compose.yml
```

## Endpoints da API

### Produtos

- `GET /api/products` - Listar todos
- `GET /api/products/:id` - Obter por ID
- `GET /api/products/category/:category` - Por categoria
- `POST /api/products` - Criar (Admin)
- `PUT /api/products/:id` - Atualizar (Admin)
- `DELETE /api/products/:id` - Deletar (Admin)

### Categorias

- `GET /api/categories` - Listar
- `POST /api/categories` - Criar (Admin)
- `PUT /api/categories/:id` - Atualizar (Admin)

### Pedidos

- `GET /api/orders` - Listar pedidos do usuário
- `GET /api/orders/:id` - Obter detalhes
- `POST /api/orders` - Criar pedido
- `PUT /api/orders/:id/status` - Atualizar status

### Promoções

- `GET /api/promotions` - Listar ativas
- `POST /api/promotions` - Criar (Admin)
- `PUT /api/promotions/:id` - Atualizar (Admin)

### Autenticação

- `POST /api/auth/register` - Registrar
- `POST /api/auth/login` - Login
- `POST /api/auth/refresh` - Refresh token

### Ifood Integration

- `POST /api/ifood/webhook` - Receber eventos
- `GET /api/ifood/orders` - Sincronizar pedidos
- `PUT /api/ifood/orders/:id/status` - Atualizar status

## Setup

### 1. Criar projeto .NET

```bash
dotnet new webapi -n PizzaDelivery.API
cd PizzaDelivery.API
```

### 2. Instalar NuGet packages

```bash
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package Swashbuckle.AspNetCore
dotnet add package AutoMapper
dotnet add package Serilog
```

### 3. Configurar banco de dados

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## Ambiente

### Development

```
appsettings.Development.json
- DB local
- CORS aberto
- Swagger habilitado
```

### Production

```
appsettings.Production.json
- DB em produção
- CORS restrito
- Logging completo
```

## Autenticação

- JWT Bearer tokens
- Roles: Admin, Manager, Customer
- Refresh token rotation
- Logout com token blacklist

## Integração Ifood

- OAuth 2.0 Authentication
- Webhook listener para pedidos
- Sincronização de horários
- Atualização de status em tempo real

## Versionamento de API

- /api/v1/...
- /api/v2/...(futuro)
