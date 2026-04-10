# Arquitetura - Pizza Delivery

## Visão Geral

```
┌─────────────────────────────────────────────────────────────┐
│                         Cliente                             │
│                   (Web/Mobile Browser)                      │
└────────────────┬────────────────────────────────────────────┘
                 │
                 │ HTTP/REST
                 │
┌────────────────┴─────────────────────────────────────────────┐
│                   Load Balancer / Reverse Proxy              │
└────────────────┬─────────────────────────────────────────────┘
                 │
       ┌─────────┼─────────┐
       │         │         │
       v         v         v
┌──────────┐ ┌──────────┐ ┌──────────┐
│ Frontend │ │ Admin    │ │   API    │
│          │ │ Panel    │ │ Backend  │
│ Next.js  │ │ Next.js  │ │ .NET     │
└────┬─────┘ └────┬─────┘ └────┬─────┘
     │            │            │
     └────────────┼────────────┘
                  │
                  │ REST API
                  │
       ┌──────────┴──────────┐
       │                     │
       v                     v
┌──────────────────┐  ┌──────────────────┐
│  Cache (Redis)   │  │  SQL Database    │
│                  │  │  (SQL Server)    │
└──────────────────┘  └──────────────────┘
       │
       └─── Ifood API Integration
```

## Stack Arquitetura

### Backend (.NET Core)

**Padrões e Princípios:**

- Clean Architecture
- Repository Pattern
- Dependency Injection
- Service Layer Pattern
- SOLID Principles

**Camadas:**

```
PizzaDelivery.API/
├── Controllers/          # HTTP endpoints
├── Services/             # Business Logic
├── Repositories/         # Data Access
├── Models/               # Domain Models
├── DTOs/                 # Data Transfer Objects
├── Middleware/           # Custom Middleware
├── Data/                 # DbContext, Migrations
└── Configuration/        # DI Setup
```

**Fluxo de Dados:**

```
Controller → Service → Repository → DbContext → Database
   ↓          ↓           ↓
Request    Business    Data        CRUD
Handler    Logic       Access
```

### Frontend (Next.js + React)

**Arquitetura:**

```
Frontend/
├── app/                  # Pages (App Router)
├── components/           # Reusable Components
├── services/             # API Services
├── hooks/                # Custom Hooks
├── store/                # Zustand State
├── types/                # TypeScript Types
└── styles/               # TailwindCSS
```

**Fluxo Component:**

```
Page
  ├─ Layout
  ├─ Header
  ├─ MainContent
  │   ├─ ProductCard
  │   ├─ CategoryTabs
  │   └─ Cart Summary
  └─ Footer
```

**State Management:**

- **Global State:** Zustand (Cart Store)
- **Server State:** React Query
- **Local State:** useState

### Admin Panel (Next.js)

Similar ao Frontend com painel gerencial.

## Banco de Dados

### Modelo Relacional

```
Users
├─ Orders (1:N)
│  ├─ OrderItems (1:N)
│  │  └─ Products (N:1)
│  └─ Promotions (N:1)
│
Categories
├─ Products (1:N)
│
Promotions
├─ PromotionProducts (1:N)
└─ Orders (1:N)
```

### Principais Tabelas

| Tabela            | Objetivo                     |
| ----------------- | ---------------------------- |
| Users             | Usuários do sistema          |
| Products          | Cardápio de pizzas           |
| Categories        | Categorias de produtos       |
| Orders            | Pedidos dos clientes         |
| OrderItems        | Itens dentro de cada pedido  |
| Promotions        | Descontos e promoções ativas |
| PromotionProducts | Relação N:N de promoções     |

## Fluxo de Pedido

```
1. Cliente navega produtos
   ↓
2. Seleciona e adiciona ao carrinho (localStorage)
   ↓
3. Clica em "Checkout"
   ↓
4. Faz login/registro (JWT Auth)
   ↓
5. Insere endereço de entrega
   ↓
6. Aplica código promocional (se houver)
   ↓
7. Confirma pedido
   ↓
8. Backend cria Order e OrderItems
   ↓
9. Integra com Ifood (se ativo)
   ↓
10. Retorna confirmação para cliente
```

## Integração Ifood

### Fluxo de Sincronização

```
Ifood Partner
    ↓
OAuth Authentication
    ↓
Get Orders from Ifood API
    ↓
Create/Update Orders in Local DB
    ↓
Webhook: Ifood → API
    ↓
Update Order Status
    ↓
Notify Customer
```

### Endpoints Ifood

- `GET /orders` - Buscar pedidos
- `PATCH /orders/{id}/status` - Atualizar status
- `POST /webhook` - Receber eventos

## Segurança

### Autenticação

```
POST /auth/login
    ↓
Validar credenciais
    ↓
Gerar JWT Token
    ↓
Return Token + Refresh Token
    ↓
Cliente armazena em localStorage
    ↓
Adiciona em Authorization header
```

### JWT Structure

```
Header: { alg: "HS256", typ: "JWT" }
Payload: {
  sub: userId,
  email: userEmail,
  role: userRole,
  exp: expirationTime
}
Signature: HMAC-SHA256(header + payload + secret)
```

### CORS

Configurado para funcionar com:

- `http://localhost:3000` (Frontend)
- `http://localhost:3001` (Admin)
- Production URLs

## Performance

### Frontend Optimization

- Image Optimization (Next.js Image)
- Code Splitting
- Dynamic Imports
- Server-side Rendering (SSR/ISR)
- Caching headers

### Backend Optimization

- Database Indexing
- Query Optimization
- Caching (Future: Redis)
- Async/await operations
- Response Compression

### Monitoramento

- Serilog para logging
- Application Insights (Future)
- Health Checks

## Deployment

### Container Strategy

```
Docker Images:
├─ backend:latest (API .NET)
├─ frontend:latest (Next.js)
└─ admin:latest (Admin Panel)

Orchestration: Docker Compose (dev)
Production: Kubernetes ou Azure Container Apps
```

### CI/CD

```
Push → GitHub Actions
    ↓
Build & Test
    ↓
Deploy to Staging
    ↓
Manual Approval
    ↓
Deploy to Production
```

## Escalabilidade

### Estratégias Futuras

1. **Horizontal Scaling:** Load Balancer + múltiplas instâncias
2. **Cache Layer:** Redis para cache de produtos/promoções
3. **Database Replication:** Master-Slave para reads
4. **CDN:** Para assets estáticos (images)
5. **Message Queue:** RabbitMQ para async operations

## Monitoramento e Logs

### Logs

- **Backend:** Serilog → Arquivo + Console
- **Frontend:** Console + Error Tracking (Future: Sentry)

### Métricas

- Request count por endpoint
- Database query performance
- API response time
- Error rates

## Backup e Recuperação

### Estratégia

1. **Database:** Full backups diários
2. **Media:** Upload para blob storage
3. **Code:** GitHub (source control)

### RTO/RPO

- RTO (Recovery Time): 4 horas
- RPO (Recovery Point): 24 horas

## Versionamento

### API Versioning

- URI-based: `/api/v1`, `/api/v2`
- Backward compatibility para versões anteriores

### Database Versioning

- Entity Framework Migrations
- Versionamento semântico

## Roadmap Futuro

- [ ] Redis Caching
- [ ] Real-time Order Tracking (SignalR)
- [ ] Mobile App (React Native)
- [ ] AI-powered Recommendations
- [ ] Payment Gateway Integration
- [ ] Multi-language Support
- [ ] Two-factor Authentication
