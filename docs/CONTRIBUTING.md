# Guia de Contribuição

Obrigado por se interessar em contribuir com o Pizza Delivery! Este documento fornece as orientações para contribuir com o projeto.

## Como Começar

1. Fork o repositório
2. Clone seu fork: `git clone https://github.com/seu-usuario/project-pizza-delivery.git`
3. Crie uma branch para sua feature: `git checkout -b feature/my-feature`
4. Faça suas mudanças
5. Commit suas mudanças: `git commit -am 'Add my feature'`
6. Push para a branch: `git push origin feature/my-feature`
7. Abra um Pull Request

## Padrões de Código

### Backend (.NET)

#### Convenções de Naming

```csharp
// Classes e Métodos: PascalCase
public class ProductService { }
public async Task<Product> GetByIdAsync(int id) { }

// Variáveis e propriedades privadas: camelCase
private string _connectionString;
private int userId;

// Propriedades públicas: PascalCase
public string Name { get; set; }
```

#### Estrutura de Classe

```csharp
public class MyService : IMyService
{
    private readonly IRepository _repository;
    private readonly ILogger<MyService> _logger;

    // Constructor
    public MyService(IRepository repository, ILogger<MyService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    // Public methods
    public async Task<Result> DoSomethingAsync()
    {
        try
        {
            // Implementation
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error message");
            throw;
        }
    }

    // Private methods
    private string HelperMethod()
    {
        return "";
    }
}
```

#### Logging

```csharp
_logger.LogInformation("Product created with ID: {ProductId}", product.Id);
_logger.LogError(ex, "Failed to create product for user {UserId}", userId);
```

### Frontend (Next.js/React)

#### Convenções de Naming

```typescript
// Componentes: PascalCase
export const ProductCard = ({ product }: ProductCardProps) => {
  return <div>{product.name}</div>;
};

// Funções e variáveis: camelCase
const handleAddToCart = () => {};
const [isLoading, setIsLoading] = useState(false);

// Constantes: UPPER_SNAKE_CASE
const MAX_ITEMS_PER_ORDER = 50;
const API_TIMEOUT = 5000;
```

#### Estrutura de Componente

```typescript
import { FC, useState } from 'react';
import { Product } from '@/types';

interface ProductCardProps {
  product: Product;
  onAddToCart: (product: Product) => void;
}

export const ProductCard: FC<ProductCardProps> = ({
  product,
  onAddToCart
}) => {
  const [isAdding, setIsAdding] = useState(false);

  const handleAddClick = async () => {
    setIsAdding(true);
    try {
      onAddToCart(product);
    } finally {
      setIsAdding(false);
    }
  };

  return (
    <div className="product-card">
      <h3>{product.name}</h3>
      <p>{product.description}</p>
      <p>R$ {product.price.toFixed(2)}</p>
      <button onClick={handleAddClick} disabled={isAdding}>
        {isAdding ? 'Adicionando...' : 'Adicionar ao Carrinho'}
      </button>
    </div>
  );
};
```

## Commit Messages

Seguir padrão Conventional Commits:

```
<type>(<scope>): <subject>

<body>

<footer>
```

Tipos válidos:

- `feat`: Nova feature
- `fix`: Correção de bug
- `docs`: Documentação
- `style`: Formatação, espaçamento (sem alteração funcional)
- `refactor`: Refatoração de código
- `test`: Testes
- `chore`: Tarefas de build, dependências

Exemplos:

```
feat(products): add product filtering by category
fix(cart): remove item from cart correctly
docs(readme): update installation instructions
test(auth): add login flow tests
```

## Pull Request

Ao abrir um PR:

1. **Título claro:**

   ```
   [Feature] Add product ratings
   [Fix] Fix cart total calculation
   ```

2. **Descrição:**
   - O que foi mudado?
   - Por quê?
   - Como testar?

3. **Checklist:**
   ```
   - [ ] Testei as mudanças localmente
   - [ ] Executei os testes automatizados
   - [ ] Atualizei documentação
   - [ ] Não há conflitos de merge
   - [ ] PR segue o padrão de código
   ```

## Testes

### Backend

```bash
cd backend
dotnet test
```

Exemplo de teste unitário:

```csharp
[Fact]
public async Task CreateProduct_WithValidData_ReturnsProduct()
{
    // Arrange
    var input = new CreateProductDto { Name = "Pizza", Price = 45 };
    var mockRepo = new Mock<IProductRepository>();

    // Act
    var service = new ProductService(mockRepo.Object);
    var result = await service.CreateAsync(input);

    // Assert
    Assert.NotNull(result);
    Assert.Equal("Pizza", result.Name);
}
```

### Frontend

```bash
cd frontend
npm test
npm run test:coverage
```

Exemplo com Jest:

```typescript
describe('ProductCard', () => {
  it('should render product name', () => {
    const product = { id: 1, name: 'Pizza' };
    const { getByText } = render(<ProductCard product={product} />);
    expect(getByText('Pizza')).toBeInTheDocument();
  });
});
```

## Checklist de Qualidade

Antes de fazer commit, verifique:

- [ ] Código segue as convenções do projeto
- [ ] Sem console.log ou debug code
- [ ] Sem commented code
- [ ] Funções possuem descrição/comentários complexos
- [ ] Testes estão passando
- [ ] Sem warnings de linting
- [ ] Commit message segue padrão

Backend:

```bash
dotnet build
dotnet test
```

Frontend:

```bash
npm run lint
npm test
npm run build
```

## Issues e Discussions

### Reportar Bug

Inclua:

- Versão do software
- Passos para reproduzir
- Comportamento esperado
- Comportamento atual
- Screenshots/logs

### Sugerir Feature

Inclua:

- Problema que resolve
- Casos de uso
- Possível implementação
- Exemplos

## Documentação

Ao adicionar nova feature, atualize:

1. `README.md` - Visão geral
2. `docs/API.md` - Se é endpoint API
3. Comentários no código
4. Arquivos de tipo (TypeScript)

## Licença

Ao contribuir, você concorda que suas contribuições sejam licenciadas sob a [MIT License](LICENSE).

## Código de Conduta

Por favor, note que nosso projeto possui um Código de Conduta. Ao participar, você esperasse aderir àquelas proteções e normas comunitários.

---

Obrigado por contribuir! 🍕
