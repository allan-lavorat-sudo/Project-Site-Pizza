# Admin Panel - Pizza Delivery

Painel administrativo completo para gerenciar pizzaria.

## Funcionalidades

### Dashboard

- 📊 Estatísticas de vendas
- 📈 Gráficos de performance
- 📉 Análise de tendências
- 🔔 Notifications de novos pedidos

### Gerenciamento de Produtos

- ➕ Criar produtos
- ✏️ Editar produtos
- 🗑️ Deletar produtos
- 🏷️ Categorizar produtos
- 💰 Definir preços
- ⭐ Gerenciar avaliações

### Gerenciamento de Categorias

- Criar categorias
- Editar categorias
- Reordenar categorias
- Ativar/desativar categorias

### Gerenciamento de Pedidos

- 📋 Listar pedidos
- 👁️ Ver detalhes
- 📍 Acompanhar status
- 💬 Notas do pedido
- 🚚 Delivery info

### Gerenciamento de Promoções

- Criar promoções
- Definir descontos (% ou valor fixo)
- Definir período válido
- Listar promoções ativas

### Relatórios

- 💹 Vendas por período
- 👥 Clientes
- 🏆 Produtos mais vendidos
- 📊 Receita comparativa

### Integração Ifood

- 🔗 Conectar com Ifood
- 📱 Sincronizar pedidos
- 🔄 Atualizar status
- 📊 Relatório de vendas Ifood

### Usuários

- 👤 Gerenciar usuários
- 🔐 Roles e permissões
- 🔑 Resetar senhas
- 📋 Auditoria de ações

## Stack

- **Next.js 14** - Framework
- **React 18** - UI Library
- **TailwindCSS** - Styling
- **Chart.js** - Gráficos
- **React Query** - Data fetching
- **Zustand** - State management

## Setup

```bash
npm install
cp .env.example .env.local
npm run dev
```

Acesso em `http://localhost:3001`

## Estrutura

```
admin-panel/
├── src/
│   ├── app/
│   │   ├── dashboard/        # Dashboard principal
│   │   ├── products/         # Gerenciar produtos
│   │   ├── categories/       # Gerenciar categorias
│   │   ├── orders/           # Gerenciar pedidos
│   │   ├── promotions/       # Gerenciar promoções
│   │   ├── reports/          # Relatórios
│   │   ├── settings/         # Configurações
│   │   └── layout.tsx
│   ├── components/
│   │   ├── Sidebar/
│   │   ├── Navbar/
│   │   ├── DataTable/
│   │   ├── Charts/
│   │   └── Form/
│   ├── services/             # API calls
│   └── types/                # TypeScript types
```

## Permissões

- **Admin:** Acesso total
- **Manager:** Gerenciar produtos, pedidos, promotions
- **Staff:** Visualizar apenas

## Segurança

- JWT Authentication
- Role-based access control
- Audit logs para ações
- Rate limiting
- CSRF protection

## Performance

- Server-side pagination
- Data caching
- Lazy loading
- Code splitting

## Responsivo

- ✅ Desktop
- ✅ Tablet
- ✅ Mobile friendly

Veja `README.md` root para mais instruções.
