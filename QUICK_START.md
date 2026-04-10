# 📋 Guia Rápido de Comandos

## 🔥 Start Rápido

### Opção 1: Com Docker (Recomendado)

```bash
# Clonar/abrir projeto
cd "Project Site Pizza"

# Iniciar todos os serviços
docker-compose up --build

# Parar
docker-compose down
```

### Opção 2: Development Local

```bash
# Terminal 1: Backend
cd backend
dotnet restore
dotnet ef database update
dotnet run

# Terminal 2: Frontend
cd frontend
npm install
npm run dev

# Terminal 3: Admin
cd admin-panel
npm install
npm run dev
```

---

## 🗄️ Banco de Dados

### Criar/Atualizar Schema

```bash
cd backend
dotnet ef migrations add MigrationName
dotnet ef database update
```

### Reset do Banco

```bash
cd backend
dotnet ef database drop
dotnet ef database update
```

### Executar Script SQL

```sql
-- SQL Server Management Studio
USE PizzaDeliveryDB
EXEC sp_GetActivePromotions
EXEC sp_GetProductsByCategory @CategoryId = 1
```

---

## 📦 Dependências

### Backend

```bash
cd backend

# Restaurar
dotnet restore

# Limpar
dotnet clean

# Build
dotnet build

# Publicar
dotnet publish -c Release
```

### Frontend/Admin

```bash
cd frontend
# ou
cd admin-panel

# Instalar
npm install

# Atualizar
npm update

# Verificar dependências desatualizadas
npm outdated

# Auditar segurança
npm audit
npm audit fix
```

---

## 🧪 Testes

### Testes Unitários Backend

```bash
cd backend
dotnet test
dotnet test /p:CollectCoverage=true
```

### Testes Frontend

```bash
cd frontend
npm test
npm run test:coverage
npm run test:watch
```

---

## 🛠️ Build & Deploy

### Build Backend

```bash
cd backend
dotnet build -c Release
dotnet publish -c Release -o publish
```

### Build Frontend

```bash
cd frontend
npm run build
npm start
```

### Build Docker Images

```bash
# Backend
docker build -t pizza-api:latest ./backend

# Frontend
docker build -t pizza-web:latest ./frontend

# Admin
docker build -t pizza-admin:latest ./admin-panel
```

---

## 🔍 Debugging

### Backend

```bash
cd backend

# Debug mode
dotnet run --configuration Debug

# Com breakpoints (Visual Studio)
# F5 para iniciar debug
# F10 para step over
# F11 para step into
```

### Frontend

```bash
cd frontend

# Dev mode com source maps
npm run dev

# Chrome DevTools: F12
# Next.js inspector: http://localhost:9229
```

---

## 📊 Logs e Monitoring

### Backend Logs

```bash
# Ver logs em arquivo
tail -f backend/logs/pizza-delivery-*.log

# Ou no Windows
Get-Content backend/logs/pizza-delivery-*.log -Tail 10 -Wait
```

### Docker Logs

```bash
# Todos os serviços
docker-compose logs -f

# Específico
docker-compose logs -f backend
docker-compose logs -f frontend
docker-compose logs -f sqlserver
```

---

## 🔐 Autenticação

### Gerar JWT Token (para testes)

```bash
# POST /auth/login
curl -X POST http://localhost:5000/api/v1/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@example.com","password":"Admin123!"}'
```

### Usar Token

```bash
# Authorization header
curl -H "Authorization: Bearer {TOKEN}" \
  http://localhost:5000/api/v1/products
```

---

## 🌐 API REST (CURL Examples)

### Listar Produtos

```bash
curl http://localhost:5000/api/v1/products
```

### Criar Pedido

```bash
curl -X POST http://localhost:5000/api/v1/orders \
  -H "Authorization: Bearer {TOKEN}" \
  -H "Content-Type: application/json" \
  -d '{
    "items": [{"productId": 1, "quantity": 2}],
    "deliveryAddress": "Rua X, 123"
  }'
```

### Listar Categorias

```bash
curl http://localhost:5000/api/v1/categories
```

### Listar Promoções Ativas

```bash
curl http://localhost:5000/api/v1/promotions/active
```

---

## 🧹 Limpeza

### Frontend

```bash
cd frontend

# Limpar node_modules
rm -rf node_modules
npm install

# Limpar .next
rm -rf .next
npm run build
```

### Backend

```bash
cd backend

# Limpar build
dotnet clean

# Limpar packages
rm -rf bin/ obj/
dotnet restore
```

### Docker

```bash
# Remover containers parados
docker-compose down -v

# Limpar images não utilizadas
docker image prune

# Limpar volumes órfãos
docker volume prune
```

---

## 🐛 Troubleshooting Commands

### Porta em Uso

```bash
# Windows
netstat -ano | findstr :5000
taskkill /PID {PID} /F

# Mac/Linux
lsof -i :5000
kill -9 {PID}
```

### Verificar Conectividade

```bash
# Backend
curl http://localhost:5000/health

# Frontend
curl http://localhost:3000

# SQL Server
sqlcmd -S localhost -U sa -P YourPassword123! -Q "SELECT 1"
```

### Verificar Ambiente

```bash
# .NET version
dotnet --version

# Node version
node -v
npm -v

# Docker version
docker --version
docker-compose --version
```

---

## 📝 Variáveis de Ambiente

### Backend

```bash
# Criar .env (Windows)
type nul > backend\.env

# Editar
notepad backend\.env

# Conteúdo (exemplo)
ConnectionStrings__DefaultConnection=Server=localhost;Database=PizzaDeliveryDB;...
JwtSettings__Secret=your-secret-key
```

### Frontend

```bash
# Criar .env.local
echo "" > frontend/.env.local

# Conteúdo (exemplo)
NEXT_PUBLIC_API_URL=http://localhost:5000/api/v1
```

---

## 📚 Documentação

```bash
# Swagger API
http://localhost:5000

# Documentação
cat docs/API.md
cat docs/SETUP.md
cat docs/ARCHITECTURE.md
```

---

## ✅ Checklist Pré-Deploy

- [ ] Testes passando (`npm test`, `dotnet test`)
- [ ] Build sem erros (`npm run build`, `dotnet build`)
- [ ] Variáveis de ambiente configuradas
- [ ] Migrations aplicadas
- [ ] Documentação atualizada
- [ ] Security audit (`npm audit`)
- [ ] Performance check
- [ ] CORS configurado
- [ ] Logs habilitados
- [ ] Backup do banco antes de deploy

---

## 🆘 Comandos de Emergência

### Resetar Tudo

```bash
# 1. Stop everything
docker-compose down -v

# 2. Remove build artifacts
cd backend && dotnet clean && rm -rf bin obj
cd ../frontend && rm -rf node_modules .next
cd ../admin-panel && rm -rf node_modules .next

# 3. Start fresh
docker-compose up --build
```

### Verificar Saúde

```bash
# Todos os containers
docker-compose ps

# Logs completos
docker-compose logs

# Health check
docker-compose exec backend curl http://localhost/health
```

---

**Dica:** Crie um arquivo `Makefile` para simplificar ainda mais os comandos:

```makefile
.PHONY: install build run test clean

install:
	cd backend && dotnet restore
	cd frontend && npm install
	cd admin-panel && npm install

build:
	cd backend && dotnet build
	cd frontend && npm run build
	cd admin-panel && npm run build

run:
	docker-compose up

test:
	cd backend && dotnet test
	cd frontend && npm test

clean:
	docker-compose down -v
	cd backend && dotnet clean
	cd frontend && rm -rf .next node_modules
	cd admin-panel && rm -rf .next node_modules
```

Então use: `make install`, `make run`, `make test`, etc.

---

**Happy coding! 🚀🍕**
