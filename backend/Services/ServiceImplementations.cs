using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using PizzaDelivery.API.DTOs;
using PizzaDelivery.API.Models;
using PizzaDelivery.API.Repositories;
using Microsoft.Extensions.Configuration;

namespace PizzaDelivery.API.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var product = await _repository.GetByIdAsync(id);
        return product != null ? _mapper.Map<ProductDto>(product) : null;
    }

    public async Task<List<ProductDto>> GetAllAsync()
    {
        var products = await _repository.GetAllAsync();
        return _mapper.Map<List<ProductDto>>(products);
    }

    public async Task<List<ProductDto>> GetByCategoryAsync(int categoryId)
    {
        var products = await _repository.GetByCategoryAsync(categoryId);
        return _mapper.Map<List<ProductDto>>(products);
    }

    public async Task<PaginatedResponse<ProductDto>> GetPaginatedAsync(int pageNumber, int pageSize)
    {
        var result = await _repository.GetPaginatedAsync(pageNumber, pageSize);
        return new PaginatedResponse<ProductDto>
        {
            Items = _mapper.Map<List<ProductDto>>(result.Items),
            TotalCount = result.TotalCount,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto dto)
    {
        var product = _mapper.Map<Product>(dto);
        var created = await _repository.CreateAsync(product);
        return _mapper.Map<ProductDto>(created);
    }

    public async Task<ProductDto> UpdateAsync(int id, UpdateProductDto dto)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product == null)
            throw new KeyNotFoundException($"Produto com ID {id} não encontrado");

        _mapper.Map(dto, product);
        var updated = await _repository.UpdateAsync(product);
        return _mapper.Map<ProductDto>(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;
    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CategoryDto?> GetByIdAsync(int id)
    {
        var category = await _repository.GetByIdAsync(id);
        return category != null ? _mapper.Map<CategoryDto>(category) : null;
    }

    public async Task<List<CategoryDto>> GetAllAsync()
    {
        var categories = await _repository.GetAllAsync();
        return _mapper.Map<List<CategoryDto>>(categories);
    }

    public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto)
    {
        var category = _mapper.Map<Category>(dto);
        var created = await _repository.CreateAsync(category);
        return _mapper.Map<CategoryDto>(created);
    }

    public async Task<CategoryDto> UpdateAsync(int id, CreateCategoryDto dto)
    {
        var category = await _repository.GetByIdAsync(id);
        if (category == null)
            throw new KeyNotFoundException($"Categoria com ID {id} não encontrada");

        _mapper.Map(dto, category);
        var updated = await _repository.UpdateAsync(category);
        return _mapper.Map<CategoryDto>(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;
    private readonly IProductRepository _productRepository;
    private readonly IPromotionRepository _promotionRepository;
    private readonly IMapper _mapper;

    public OrderService(
        IOrderRepository repository,
        IProductRepository productRepository,
        IPromotionRepository promotionRepository,
        IMapper mapper)
    {
        _repository = repository;
        _productRepository = productRepository;
        _promotionRepository = promotionRepository;
        _mapper = mapper;
    }

    public async Task<OrderDto?> GetByIdAsync(int id)
    {
        var order = await _repository.GetByIdAsync(id);
        return order != null ? _mapper.Map<OrderDto>(order) : null;
    }

    public async Task<List<OrderDto>> GetUserOrdersAsync(int userId)
    {
        var orders = await _repository.GetByUserIdAsync(userId);
        return _mapper.Map<List<OrderDto>>(orders);
    }

    public async Task<OrderDto> CreateAsync(CreateOrderDto dto, int userId)
    {
        var order = new Order
        {
            OrderNumber = $"PD{DateTime.UtcNow:yyyyMMddHHmmss}",
            UserId = userId,
            Status = OrderStatus.Pending,
            DeliveryAddress = dto.DeliveryAddress,
            DeliveryNotes = dto.DeliveryNotes,
            DeliveryFee = 5.00m, // Default delivery fee
            PromotionId = dto.PromotionId
        };

        decimal totalAmount = 0;

        foreach (var item in dto.Items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId);
            if (product == null)
                throw new KeyNotFoundException($"Produto {item.ProductId} não encontrado");

            order.OrderItems.Add(new OrderItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = product.Price,
                Notes = item.Notes
            });

            totalAmount += product.Price * item.Quantity;
        }

        order.TotalAmount = totalAmount + order.DeliveryFee;

        // Apply promotion if exists
        if (dto.PromotionId.HasValue)
        {
            var promotion = await _promotionRepository.GetByIdAsync(dto.PromotionId.Value);
            if (promotion != null && promotion.IsActive &&
                (promotion.MinimumOrderValue == null || totalAmount >= promotion.MinimumOrderValue))
            {
                if (promotion.DiscountAmount.HasValue)
                {
                    order.DiscountAmount = promotion.DiscountAmount;
                    order.TotalAmount -= promotion.DiscountAmount.Value;
                }
                else if (promotion.DiscountPercentage.HasValue)
                {
                    var discountAmount = totalAmount * (promotion.DiscountPercentage.Value / 100);
                    order.DiscountAmount = discountAmount;
                    order.TotalAmount -= discountAmount;
                }
            }
        }

        var created = await _repository.CreateAsync(order);
        return _mapper.Map<OrderDto>(created);
    }

    public async Task<OrderDto> UpdateStatusAsync(int orderId, OrderStatus status)
    {
        var order = await _repository.GetByIdAsync(orderId);
        if (order == null)
            throw new KeyNotFoundException($"Pedido {orderId} não encontrado");

        order.Status = status;

        if (status == OrderStatus.Delivered)
            order.CompletedAt = DateTime.UtcNow;
        else if (status == OrderStatus.Cancelled)
            order.CancelledAt = DateTime.UtcNow;

        var updated = await _repository.UpdateAsync(order);
        return _mapper.Map<OrderDto>(updated);
    }

    public async Task<PaginatedResponse<OrderDto>> GetPaginatedAsync(int pageNumber, int pageSize)
    {
        var result = await _repository.GetPaginatedAsync(pageNumber, pageSize);
        return new PaginatedResponse<OrderDto>
        {
            Items = _mapper.Map<List<OrderDto>>(result.Items),
            TotalCount = result.TotalCount,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }

    public async Task<List<OrderDto>> GetPendingOrdersAsync()
    {
        var orders = await _repository.GetPendingOrdersAsync();
        return _mapper.Map<List<OrderDto>>(orders);
    }
}

public class AuthService : IAuthService
{
    private readonly IUserRepository _repository;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public AuthService(IUserRepository repository, IConfiguration configuration, IMapper mapper)
    {
        _repository = repository;
        _configuration = configuration;
        _mapper = mapper;
    }

    public async Task<AuthResponseDto> RegisterAsync(UserRegisterDto dto)
    {
        var existingUser = await _repository.GetByEmailAsync(dto.Email);
        if (existingUser != null)
            throw new InvalidOperationException("Email já cadastrado");

        if (dto.Password != dto.ConfirmPassword)
            throw new InvalidOperationException("Senhas não conferem");

        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = "Customer"
        };

        var created = await _repository.CreateAsync(user);
        var token = GenerateJwtToken(created);
        var refreshToken = GenerateRefreshToken();

        return new AuthResponseDto
        {
            AccessToken = token,
            RefreshToken = refreshToken,
            User = _mapper.Map<UserDto>(created)
        };
    }

    public async Task<AuthResponseDto> LoginAsync(UserLoginDto dto)
    {
        var user = await _repository.GetByEmailAsync(dto.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Email ou senha inválidos");

        var token = GenerateJwtToken(user);
        var refreshToken = GenerateRefreshToken();

        return new AuthResponseDto
        {
            AccessToken = token,
            RefreshToken = refreshToken,
            User = _mapper.Map<UserDto>(user)
        };
    }

    public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken)
    {
        // In a production app, validate the refresh token from database
        throw new NotImplementedException();
    }

    public async Task<bool> LogoutAsync(int userId)
    {
        // Implement token blacklist
        return true;
    }

    private string GenerateJwtToken(User user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]!));
        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(jwtSettings["ExpiryMinutes"]!)),
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}

public class PromotionService : IPromotionService
{
    private readonly IPromotionRepository _repository;
    private readonly IMapper _mapper;

    public PromotionService(IPromotionRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PromotionDto?> GetByIdAsync(int id)
    {
        var promotion = await _repository.GetByIdAsync(id);
        return promotion != null ? _mapper.Map<PromotionDto>(promotion) : null;
    }

    public async Task<List<PromotionDto>> GetActiveAsync()
    {
        var promotions = await _repository.GetActiveAsync();
        return _mapper.Map<List<PromotionDto>>(promotions);
    }

    public async Task<List<PromotionDto>> GetAllAsync()
    {
        var promotions = await _repository.GetAllAsync();
        return _mapper.Map<List<PromotionDto>>(promotions);
    }

    public async Task<PromotionDto> CreateAsync(CreatePromotionDto dto)
    {
        var promotion = _mapper.Map<Promotion>(dto);
        var created = await _repository.CreateAsync(promotion);
        return _mapper.Map<PromotionDto>(created);
    }

    public async Task<PromotionDto> UpdateAsync(int id, CreatePromotionDto dto)
    {
        var promotion = await _repository.GetByIdAsync(id);
        if (promotion == null)
            throw new KeyNotFoundException($"Promoção {id} não encontrada");

        _mapper.Map(dto, promotion);
        var updated = await _repository.UpdateAsync(promotion);
        return _mapper.Map<PromotionDto>(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}

public class IfoodService : IIfoodService
{
    private readonly IConfiguration _configuration;
    private readonly IOrderRepository _orderRepository;
    private readonly IHttpClientFactory _httpClientFactory;

    public IfoodService(
        IConfiguration configuration,
        IOrderRepository orderRepository,
        IHttpClientFactory httpClientFactory)
    {
        _configuration = configuration;
        _orderRepository = orderRepository;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<bool> AuthenticateAsync(string authCode)
    {
        // Implement Ifood OAuth authentication
        return true;
    }

    public async Task<bool> ProcessWebhookAsync(IfoodOrderEventDto eventData)
    {
        var order = await _orderRepository.GetByIfoodOrderIdAsync(eventData.OrderId);
        if (order == null) return false;

        // Update order status based on Ifood event
        return true;
    }

    public async Task<bool> UpdateOrderStatusAsync(string ifoodOrderId, string status)
    {
        // Send status update to Ifood API
        return true;
    }

    public async Task<bool> SyncMenuAsync()
    {
        // Sync menu with Ifood
        return true;
    }

    public async Task<string> GetAuthorizationUrlAsync()
    {
        var clientId = _configuration["IfoodSettings:ClientId"];
        var redirectUri = _configuration["IfoodSettings:RedirectUri"];
        return $"https://api.ifood.com.br/oauth/authorize?client_id={clientId}&redirect_uri={redirectUri}";
    }
}
