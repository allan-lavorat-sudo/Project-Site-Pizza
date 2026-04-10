namespace PizzaDelivery.API.Repositories;

using PizzaDelivery.API.Models;
using PizzaDelivery.API.DTOs;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(int id);
    Task<List<Product>> GetAllAsync();
    Task<List<Product>> GetByCategoryAsync(int categoryId);
    Task<PaginatedResponse<Product>> GetPaginatedAsync(int pageNumber, int pageSize);
    Task<Product> CreateAsync(Product product);
    Task<Product> UpdateAsync(Product product);
    Task<bool> DeleteAsync(int id);
}

public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(int id);
    Task<List<Category>> GetAllAsync();
    Task<Category> CreateAsync(Category category);
    Task<Category> UpdateAsync(Category category);
    Task<bool> DeleteAsync(int id);
}

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(int id);
    Task<List<Order>> GetByUserIdAsync(int userId);
    Task<Order?> GetByOrderNumberAsync(string orderNumber);
    Task<Order?> GetByIfoodOrderIdAsync(string ifoodOrderId);
    Task<Order> CreateAsync(Order order);
    Task<Order> UpdateAsync(Order order);
    Task<List<Order>> GetPendingOrdersAsync();
    Task<PaginatedResponse<Order>> GetPaginatedAsync(int pageNumber, int pageSize);
}

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByEmailAsync(string email);
    Task<User> CreateAsync(User user);
    Task<User> UpdateAsync(User user);
    Task<bool> DeleteAsync(int id);
}

public interface IPromotionRepository
{
    Task<Promotion?> GetByIdAsync(int id);
    Task<List<Promotion>> GetActiveAsync();
    Task<List<Promotion>> GetAllAsync();
    Task<Promotion> CreateAsync(Promotion promotion);
    Task<Promotion> UpdateAsync(Promotion promotion);
    Task<bool> DeleteAsync(int id);
}
