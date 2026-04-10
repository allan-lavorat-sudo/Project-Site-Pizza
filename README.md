# 🍕 Site de Pizzaria - Pizza Delivery

Um site completo e moderno de pizzaria com delivery via Ifood. Construído com as melhores tecnologias do mercado.

## 📋 Stack Tecnológico

### Backend

- **ASP.NET Core 8** - Framework web robusto
- **Entity Framework Core** - ORM para banco de dados
- **SQL Server/PostgreSQL** - Banco de dados
- **JWT Authentication** - Autenticação segura
- **Swagger/OpenAPI** - Documentação de API

### Frontend

- **Next.js 14** - Framework React fullstack
- **React** - UI library
- **TailwindCSS** - Styling
- **TypeScript** - Type safety
- **Axios** - HTTP client
- **React Query** - State management

### Admin Panel

- **Next.js 14** - Admin dashboard
- **React Admin** - Interface administrativa
- **Chart.js** - Gráficos e analytics

### Banco de Dados

- **SQL Server** ou **PostgreSQL**
- **Entity Framework Core Migrations**

## 📁 Estrutura do Projeto

```
Project Site Pizza/
├── backend/                 # API .NET Core
├── frontend/               # Site público (Next.js)
├── admin-panel/            # Painel administrativo
├── database/               # Scripts SQL e migrations
├── docs/                   # Documentação
└── README.md
```

## 🚀 Funcionalidades

### Cliente

- ✅ Catálogo de Pizzas (Salgadas, Doces)
- ✅ Bebidas e Sobremesas
- ✅ Promoções em Destaque
- ✅ Carrinho de Compras
- ✅ Autenticação de Usuário
- ✅ Histórico de Pedidos
- ✅ Integração Ifood

### Admin/Gerente

- ✅ Gerenciar Produtos (CRUD)
- ✅ Gerenciar Categorias
- ✅ Gerenciar Promoções
- ✅ Dashboard de Vendas
- ✅ Relatórios
- ✅ Gerenciar Pedidos
- ✅ Integração com Ifood API

## 🛠️ Instalação e Setup

### Pré-requisitos

- .NET 8 SDK
- Node.js 18+
- npm ou yarn
- SQL Server ou PostgreSQL
- Git

### Backend Setup

```bash
cd backend
dotnet restore
dotnet ef database update
dotnet run
```

### Frontend Setup

```bash
cd frontend
npm install
npm run dev
```

### Admin Panel Setup

```bash
cd admin-panel
npm install
npm run dev
```

## 📝 Documentação

Veja a pasta `docs/` para:

- Arquitetura do projeto
- Guia de API
- Configuração de banco de dados
- Setup de desenvolvimento

## 🔐 Variáveis de Ambiente

Crie um arquivo `.env.local` em cada diretório (backend, frontend, admin-panel):

```
# Backend
API_KEY=your_api_key
DB_CONNECTION=your_connection_string
IFOOD_API_KEY=your_ifood_key
JWT_SECRET=your_jwt_secret

# Frontend/Admin
NEXT_PUBLIC_API_URL=http://localhost:5000
NEXT_PUBLIC_IFOOD_PARTNER_ID=your_ifood_id
```

## 🔗 Integração Ifood

- API para receber pedidos
- Webhook para atualizações de status
- Autenticação OAuth
- Sincronização de menú

## 📊 Dashboard Admin

- Analytics de vendas
- Gráficos de performance
- Gerenciamento de usuários
- Logs de sistema

## 📱 Responsivo

- ✅ Mobile First Design
- ✅ Desktop otimizado
- ✅ Tablet compatible

## 🧪 Testes

- Unit Tests (.NET xUnit)
- Integration Tests
- E2E Tests (Cypress)

## 🚢 Deploy

- Docker containers
- CI/CD com GitHub Actions
- Azure App Service ou similar

## 📄 Licença

MIT

## 👨‍💻 Desenvolvido com ❤️

---

**Status do Projeto**: Em desenvolvimento 🚧
