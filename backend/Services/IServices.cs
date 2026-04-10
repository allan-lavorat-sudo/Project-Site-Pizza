namespace PizzaDelivery.API.Services;

using PizzaDelivery.API.DTOs;
using PizzaDelivery.API.Models;

public interface IProductService
{
    Task<ProductDto?> GetByIdAsync(int id);
    Task<List<ProductDto>> GetAllAsync();
    Task<List<ProductDto>> GetByCategoryAsync(int categoryId);
    Task<PaginatedResponse<ProductDto>> GetPaginatedAsync(int pageNumber, int pageSize);
    Task<ProductDto> CreateAsync(CreateProductDto dto);
    Task<ProductDto> UpdateAsync(int id, UpdateProductDto dto);
    Task<bool> DeleteAsync(int id);
}

public interface ICategoryService
{
    Task<CategoryDto?> GetByIdAsync(int id);
    Task<List<CategoryDto>> GetAllAsync();
    Task<CategoryDto> CreateAsync(CreateCategoryDto dto);
    Task<CategoryDto> UpdateAsync(int id, CreateCategoryDto dto);
    Task<bool> DeleteAsync(int id);
}

public interface IOrderService
{
    Task<OrderDto?> GetByIdAsync(int id);
    Task<List<OrderDto>> GetUserOrdersAsync(int userId);
    Task<OrderDto> CreateAsync(CreateOrderDto dto, int userId);
    Task<OrderDto> UpdateStatusAsync(int orderId, OrderStatus status);
    Task<PaginatedResponse<OrderDto>> GetPaginatedAsync(int pageNumber, int pageSize);
    Task<List<OrderDto>> GetPendingOrdersAsync();
}

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(UserRegisterDto dto);
    Task<AuthResponseDto> LoginAsync(UserLoginDto dto);
    Task<AuthResponseDto> RefreshTokenAsync(string refreshToken);
    Task<bool> LogoutAsync(int userId);
}

public interface IPromotionService
{
    Task<PromotionDto?> GetByIdAsync(int id);
    Task<List<PromotionDto>> GetActiveAsync();
    Task<List<PromotionDto>> GetAllAsync();
    Task<PromotionDto> CreateAsync(CreatePromotionDto dto);
    Task<PromotionDto> UpdateAsync(int id, CreatePromotionDto dto);
    Task<bool> DeleteAsync(int id);
}

public interface IIfoodService
{
    Task<bool> AuthenticateAsync(string authCode);
    Task<bool> ProcessWebhookAsync(IfoodOrderEventDto eventData);
    Task<bool> UpdateOrderStatusAsync(string ifoodOrderId, string status);
    Task<bool> SyncMenuAsync();
    Task<string> GetAuthorizationUrlAsync();
}
