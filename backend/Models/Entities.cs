namespace PizzaDelivery.API.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public decimal Rating { get; set; } = 0;
    
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Relationships
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public ICollection<PromotionProduct> PromotionProducts { get; set; } = new List<PromotionProduct>();
}

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string IconUrl { get; set; } = string.Empty;
    public int Order { get; set; }
    
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Relationships
    public ICollection<Product> Products { get; set; } = new List<Product>();
}

public class Order
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    
    public decimal TotalAmount { get; set; }
    public decimal DeliveryFee { get; set; }
    public decimal? DiscountAmount { get; set; }
    
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public string? IfoodOrderId { get; set; }
    
    public string DeliveryAddress { get; set; } = string.Empty;
    public string? DeliveryNotes { get; set; }
    public string? PhoneNumber { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
    public DateTime? CancelledAt { get; set; }
    
    // Relationships
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public int? PromotionId { get; set; }
    public Promotion? Promotion { get; set; }
}

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
    
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string? Notes { get; set; }
}

public class User
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    
    public string? Document { get; set; } // CPF
    public string? DefaultAddress { get; set; }
    public string? DefaultAddressNumber { get; set; }
    public string? DefaultAddressComplement { get; set; }
    public string? DefaultCity { get; set; }
    public string? DefaultZipCode { get; set; }
    
    public string Role { get; set; } = "Customer"; // Admin, Manager, Customer
    
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Relationships
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}

public class Promotion
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    
    public decimal? DiscountPercentage { get; set; }
    public decimal? DiscountAmount { get; set; }
    public decimal? MinimumOrderValue { get; set; }
    
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    public bool IsActive { get; set; } = true;
    public int DisplayOrder { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Relationships
    public ICollection<PromotionProduct> PromotionProducts { get; set; } = new List<PromotionProduct>();
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}

public class PromotionProduct
{
    public int Id { get; set; }
    public int PromotionId { get; set; }
    public Promotion Promotion { get; set; } = null!;
    
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    
    public decimal? PromotionPrice { get; set; }
}

public enum OrderStatus
{
    Pending = 0,
    Confirmed = 1,
    Preparing = 2,
    Ready = 3,
    OutForDelivery = 4,
    Delivered = 5,
    Cancelled = 6
}
