# API Documentation

## Base URL

```
http://localhost:5000/api/v1
```

## Authentication

Token JWT no header:

```
Authorization: Bearer {token}
```

## Endpoints

### Autenticação

#### Registro

```
POST /auth/register
Content-Type: application/json

{
  "fullName": "João Silva",
  "email": "joao@example.com",
  "phoneNumber": "11999999999",
  "password": "Password123!",
  "confirmPassword": "Password123!"
}

Response:
{
  "success": true,
  "data": {
    "accessToken": "token_jwt",
    "refreshToken": "refresh_token",
    "user": {
      "id": 1,
      "fullName": "João Silva",
      "email": "joao@example.com",
      "phoneNumber": "11999999999",
      "role": "Customer"
    }
  }
}
```

#### Login

```
POST /auth/login
Content-Type: application/json

{
  "email": "joao@example.com",
  "password": "Password123!"
}

Response:
{
  "success": true,
  "data": {
    "accessToken": "token_jwt",
    "refreshToken": "refresh_token",
    "user": { ... }
  }
}
```

### Produtos

#### Listar Todos

```
GET /products
```

#### Buscar por Categoria

```
GET /products/category/{categoryId}
```

#### Obter Detalhes

```
GET /products/{id}
```

#### Criar (Admin/Manager)

```
POST /products
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "Pizza Margherita",
  "description": "Mozzarella fresca...",
  "price": 45.00,
  "imageUrl": "/images/margherita.jpg",
  "categoryId": 1
}
```

#### Atualizar (Admin/Manager)

```
PUT /products/{id}
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "Pizza Margherita",
  "description": "Mozzarella fresca...",
  "price": 45.00,
  "imageUrl": "/images/margherita.jpg",
  "categoryId": 1,
  "isActive": true
}
```

#### Deletar (Admin)

```
DELETE /products/{id}
Authorization: Bearer {token}
```

### Categorias

#### Listar Todas

```
GET /categories
```

#### Obter Detalhes

```
GET /categories/{id}
```

#### Criar (Admin)

```
POST /categories
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "Pizza Salgada",
  "description": "Pizzas salgadas deliciosas",
  "iconUrl": "/icons/salty.png"
}
```

#### Atualizar (Admin)

```
PUT /categories/{id}
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "Pizza Salgada",
  "description": "Pizzas salgadas deliciosas",
  "iconUrl": "/icons/salty.png"
}
```

#### Deletar (Admin)

```
DELETE /categories/{id}
Authorization: Bearer {token}
```

### Pedidos

#### Criar Pedido

```
POST /orders
Authorization: Bearer {token}
Content-Type: application/json

{
  "items": [
    {
      "productId": 1,
      "quantity": 2,
      "notes": "Sem cebola"
    }
  ],
  "deliveryAddress": "Rua Principal, 123",
  "deliveryNotes": "Deixar na portaria",
  "promotionId": 1
}

Response:
{
  "success": true,
  "data": {
    "id": 1,
    "orderNumber": "PD20240101120000",
    "totalAmount": 105.00,
    "deliveryFee": 5.00,
    "discountAmount": 10.00,
    "status": "Pending",
    "deliveryAddress": "Rua Principal, 123",
    "createdAt": "2024-01-01T12:00:00Z",
    "items": [...]
  }
}
```

#### Listar Pedidos do Usuário

```
GET /orders
Authorization: Bearer {token}
```

#### Obter Detalhes do Pedido

```
GET /orders/{id}
Authorization: Bearer {token}
```

#### Atualizar Status (Admin/Manager)

```
PUT /orders/{id}/status
Authorization: Bearer {token}
Content-Type: application/json

{
  "status": "Preparing"
}

// Status disponíveis:
// Pending, Confirmed, Preparing, Ready, OutForDelivery, Delivered, Cancelled
```

#### Listar Pedidos Pendentes (Admin/Manager)

```
GET /orders/pending
Authorization: Bearer {token}
```

### Promoções

#### Listar Promoções Ativas

```
GET /promotions/active
```

#### Listar Todas as Promoções

```
GET /promotions
```

#### Obter Detalhes

```
GET /promotions/{id}
```

#### Criar (Admin)

```
POST /promotions
Authorization: Bearer {token}
Content-Type: application/json

{
  "title": "20% OFF em Pizzas Doces",
  "description": "20% de desconto em toda pizza doce",
  "discountPercentage": 20,
  "minimumOrderValue": 30.00,
  "startDate": "2024-01-01T00:00:00Z",
  "endDate": "2024-12-31T23:59:59Z"
}
```

#### Atualizar (Admin)

```
PUT /promotions/{id}
Authorization: Bearer {token}
Content-Type: application/json

{ ... }
```

#### Deletar (Admin)

```
DELETE /promotions/{id}
Authorization: Bearer {token}
```

### Ifood Integration

#### Webhook para Pedidos Ifood

```
POST /ifood/webhook
Content-Type: application/json
X-IFOOD-Signature: {signature}

{
  "orderId": "ifood_order_id",
  "status": "CONFIRMED",
  "data": { ... }
}
```

#### Obter URL de Autenticação

```
GET /ifood/auth-url
```

#### Autenticar com Ifood

```
POST /ifood/authenticate
Content-Type: application/json

{
  "authCode": "authorization_code"
}
```

## Status Codes

- `200` - OK
- `201` - Created
- `400` - Bad Request
- `401` - Unauthorized
- `403` - Forbidden
- `404` - Not Found
- `500` - Internal Server Error

## Erros

Resposta de erro padrão:

```json
{
  "success": false,
  "message": "Descrição do erro",
  "errors": ["Erro 1", "Erro 2"]
}
```

## Rate Limiting

- 100 requisições por minuto por IP
- 1000 requisições por hora por usuário autenticado

## Versionamento

API usa versioning por URL:

- Versão atual: `/api/v1`
- Futuras: `/api/v2`, `/api/v3`, etc.
