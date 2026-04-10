# Guia de Setup - Pizza Delivery

## Pré-requisitos

- **.NET 8 SDK** - [Download](https://dotnet.microsoft.com/download/dotnet/8.0)
- **Node.js 18+** - [Download](https://nodejs.org/)
- **SQL Server 2019+** ou **PostgreSQL 14+** - [Download](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- **Git** - [Download](https://git-scm.com/)
- **Visual Studio Code** ou **Visual Studio 2022**

## Instalação Local

### 1. Clonar o Repositório

```bash
git clone <repository-url>
cd "Project Site Pizza"
```

### 2. Setup do Backend (.NET)

#### Windows

```bash
cd backend

# Restaurar dependências
dotnet restore

# Aplicar migrações do banco
dotnet ef database update

# Executar a aplicação
dotnet run
```

A API estará disponível em `http://localhost:5000`

#### macOS/Linux

```bash
cd backend
dotnet restore
dotnet ef database update
dotnet run
```

### 3. Setup do Frontend (Next.js)

```bash
cd frontend

# Instalar dependências
npm install

# Criar arquivo de ambiente
cp .env.example .env.local

# Editar .env.local com as variáveis corretas
# NEXT_PUBLIC_API_URL=http://localhost:5000/api/v1

# Executar em desenvolvimento
npm run dev
```

Frontend estará em `http://localhost:3000`

### 4. Setup do Admin Panel

```bash
cd admin-panel

npm install
cp .env.example .env.local
npm run dev
```

Admin estará em `http://localhost:3001`

## Configuração do Banco de Dados

### SQL Server

1. Abrir SQL Server Management Studio
2. Conectar ao servidor local
3. Executar o script: `database/init.sql`
4. Banco será criado com tabelas e dados iniciais

### String de Conexão

No arquivo `backend/appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=PizzaDeliveryDB;User Id=sa;Password=YourPassword123!;Encrypt=false;"
}
```

## Variáveis de Ambiente

### Backend (`backend/appsettings.json`)

```json
{
  "JwtSettings": {
    "Secret": "your-super-secret-key-change-in-production-min-32-chars",
    "Issuer": "pizzadelivery.api",
    "Audience": "pizzadelivery.clients",
    "ExpiryMinutes": 60
  },
  "IfoodSettings": {
    "ApiBaseUrl": "https://api.ifood.com.br",
    "PartnerApiToken": "your_ifood_token",
    "MerchantId": "your_merchant_id"
  }
}
```

### Frontend (`frontend/.env.local`)

```
NEXT_PUBLIC_API_URL=http://localhost:5000/api/v1
NEXT_PUBLIC_IFOOD_PARTNER_ID=your_partner_id
NEXTAUTH_SECRET=your_secret
```

### Admin (`admin-panel/.env.local`)

```
NEXT_PUBLIC_API_URL=http://localhost:5000/api/v1
```

## Com Docker Compose

### Requisitos

- Docker
- Docker Compose

### Executar

```bash
# Construir e iniciar todos os serviços
docker-compose up --build

# Modo background
docker-compose up -d

# Parar
docker-compose down

# Ver logs
docker-compose logs -f
```

Será criado:

- API em `http://localhost:5000`
- Frontend em `http://localhost:3000`
- Admin em `http://localhost:3001`
- SQL Server em `localhost:1433`

## Dados Padrão

O banco é criado com dados iniciais:

**Categorias:**

- Pizza Salgada
- Pizza Doce
- Bebidas
- Sobremesas

**Promoções (Demo):**

- Combo 2 Pizzas + Bebida (-R$15)
- 20% OFF em Pizzas Doces
- Frete Grátis acima de R$50

## Testes

### Backend

```bash
cd backend

# Executar testes
dotnet test

# Com cobertura
dotnet test /p:CollectCoverage=true
```

### Frontend

```bash
cd frontend

# Testes com Jest
npm run test

# Com cobertura
npm run test:coverage
```

## Build para Produção

### Backend

```bash
cd backend
dotnet publish -c Release -o publish
```

### Frontend

```bash
cd frontend
npm run build
npm start
```

## Troubleshooting

### Erro de Conexão com BD

**Erro:** "Cannot open database 'PizzaDeliveryDB'"

**Solução:**

1. Verificar se SQL Server está rodando
2. Executar `dotnet ef database update` novamente
3. Verificar string de conexão em `appsettings.json`

### Porta 5000 já está em uso

**Erro:** "Address already in use"

**Solução:**

```bash
# Encontrar processo usando a porta
netstat -ano | findstr :5000

# Matar processo (Windows)
taskkill /PID {PID} /F

# Ou executar em porta diferente
dotnet run --urls="http://localhost:5001"
```

### Erro de CORS

**Erro:** "Access to XMLHttpRequest blocked by CORS"

**Solução:**
Adicionar origem no `Program.cs`:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:3000", "http://localhost:3001")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
```

### node_modules muito grande

**Solução:** Usar `.gitignore`

```bash
echo "node_modules/" >> .gitignore
```

## Integração com Ifood

### Setup Ifood API

1. Acessar [Ifood Partner](https://partner.ifood.com.br)
2. Criar aplicação in your account
3. Gerar credenciais OAuth
4. Adicionar ao `appsettings.json`:

```json
"IfoodSettings": {
  "ClientId": "seu_client_id",
  "ClientSecret": "seu_client_secret",
  "MerchantId": "seu_merchant_id"
}
```

### Webhook Ifood

Configurar webhook em Partner com URL:

```
https://sua-api.com/api/v1/ifood/webhook
```

## Deploy

Veja `docs/DEPLOYMENT.md` para instruções de deploy em produção.

## Suporte

- Documentação API: `docs/API.md`
- Arquitetura: `docs/ARCHITECTURE.md`
- Issue Tracker: GitHub Issues
