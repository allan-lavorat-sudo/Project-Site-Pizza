# Frontend - Site Pizza Delivery

Site de E-commerce de pizzaria com React e Next.js 14.

## Estrutura

```
frontend/
├── src/
│   ├── app/
│   │   ├── layout.tsx           # Root layout
│   │   ├── page.tsx             # Home page
│   │   ├── menu/                # Menu browsing
│   │   ├── cart/                # Shopping cart
│   │   ├── checkout/            # Checkout process
│   │   ├── orders/              # Order history
│   │   ├── auth/                # Authentication pages
│   │   └── admin/               # Admin dashboard (if included)
│   ├── components/
│   │   ├── Header/
│   │   ├── Footer/
│   │   ├── ProductCard/
│   │   ├── CategoryTabs/
│   │   ├── Cart/
│   │   └── PromotionBanner/
│   ├── hooks/
│   │   ├── useCart.ts
│   │   ├── useAuth.ts
│   │   └── useFetch.ts
│   ├── services/
│   │   ├── api.ts
│   │   ├── auth.service.ts
│   │   ├── product.service.ts
│   │   ├── order.service.ts
│   │   └── ifood.service.ts
│   ├── context/
│   │   ├── AuthContext.tsx
│   │   └── CartContext.tsx
│   ├── types/
│   │   └── index.ts
│   └── styles/
│       └── globals.css
├── public/
│   ├── images/
│   └── icons/
├── package.json
├── npm run dev          # Desenvolvimento
├── npm run build        # Build production
├── npm start            # Run production
└── next.config.js
```

## Tecnologias

- **Next.js 14** - Framework React
- **React 18** - UI Library
- **TypeScript** - Type Safety
- **TailwindCSS** - Styling
- **React Query** - Data fetching & caching
- **Zustand** - State management
- **Axios** - HTTP client
- **Next-auth** - Authentication

## Funcionalidades

### Cliente

✅ Visualizar cardápio por categoria
✅ Filtro de produtos
✅ Carrinho de compras (localStorage)
✅ Checkout com endereço
✅ Login/Registro
✅ Histórico de pedidos
✅ Integração com Ifood
✅ Promoções em destaque
✅ Busca de produtos
✅ Avaliações de produtos

## Setup

```bash
# Instalar dependências
npm install

# Variáveis de ambiente
cp .env.example .env.local

# Desenvolvimento
npm run dev

# Build
npm run build
npm start
```

## .env.local (Exemplo)

```
NEXT_PUBLIC_API_URL=http://localhost:5000/api/v1
NEXT_PUBLIC_IFOOD_PARTNER_ID=seu_partner_id
NEXTAUTH_SECRET=sua_secret_key
NEXTAUTH_URL=http://localhost:3000
```

## Páginas principais

### Home (`/`)

- Banner com promoções
- Categorias em destaque
- Produtos populares
- Call-to-action para ordenar

### Menu (`/menu`)

- Visualizar todos os produtos
- Filtrar por categoria
- Buscar produtos
- Adicionar ao carrinho

### Carrinho (`/cart`)

- Visualizar itens
- Editar quantidades
- Remover itens
- Ver total com frete
- Proceder para checkout

### Checkout (`/checkout`)

- Endereço de entrega
- Método de pagamento
- Código promocional
- Sumário de pedido
- Confirmar pedido

### Meus Pedidos (`/orders`)

- Listar pedidos do usuário
- Status de cada pedido
- Detalhes do trabalho

### Login/Registro (`/auth`)

- Login com email/senha
- Registro de novo usuário
- Recuperação de senha (futuro)
- Login com redes sociais (futuro)

## Integração com API

Todos os arquivo de serviço em `/services` fazem requisições para a API .NET backend.

## Performance

- Image Optimization (Next.js Image)
- Code Splitting
- Dynamic Imports
- SSG / ISR para static pages

## SEO

- Meta tags otimizadas
- Open Graph
- Structured Data
- Sitemap.xml
- robots.txt

## Segurança

- HTTPS only (production)
- CORS configurado
- JWT tokens no localStorage
- Validation em formulários
- XSS Protection com DOMPurify (opcional)
