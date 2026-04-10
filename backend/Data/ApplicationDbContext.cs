using Microsoft.EntityFrameworkCore;
using PizzaDelivery.API.Models;

namespace PizzaDelivery.API.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Promotion> Promotions { get; set; }
    public DbSet<PromotionProduct> PromotionProducts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Product Configuration
        modelBuilder.Entity<Product>()
            .HasKey(p => p.Id);
        modelBuilder.Entity<Product>()
            .Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);
        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasPrecision(10, 2);
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        // Category Configuration
        modelBuilder.Entity<Category>()
            .HasKey(c => c.Id);
        modelBuilder.Entity<Category>()
            .Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        // User Configuration
        modelBuilder.Entity<User>()
            .HasKey(u => u.Id);
        modelBuilder.Entity<User>()
            .Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(256);
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Order Configuration
        modelBuilder.Entity<Order>()
            .HasKey(o => o.Id);
        modelBuilder.Entity<Order>()
            .Property(o => o.OrderNumber)
            .IsRequired()
            .HasMaxLength(50);
        modelBuilder.Entity<Order>()
            .Property(o => o.TotalAmount)
            .HasPrecision(10, 2);
        modelBuilder.Entity<Order>()
            .HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Order>()
            .HasOne(o => o.Promotion)
            .WithMany(p => p.Orders)
            .HasForeignKey(o => o.PromotionId)
            .OnDelete(DeleteBehavior.SetNull);

        // OrderItem Configuration
        modelBuilder.Entity<OrderItem>()
            .HasKey(oi => oi.Id);
        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Product)
            .WithMany(p => p.OrderItems)
            .HasForeignKey(oi => oi.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // Promotion Configuration
        modelBuilder.Entity<Promotion>()
            .HasKey(p => p.Id);
        modelBuilder.Entity<Promotion>()
            .Property(p => p.Title)
            .IsRequired()
            .HasMaxLength(200);
        modelBuilder.Entity<Promotion>()
            .Property(p => p.DiscountPercentage)
            .HasPrecision(5, 2);

        // PromotionProduct Configuration
        modelBuilder.Entity<PromotionProduct>()
            .HasKey(pp => pp.Id);
        modelBuilder.Entity<PromotionProduct>()
            .HasOne(pp => pp.Promotion)
            .WithMany(p => p.PromotionProducts)
            .HasForeignKey(pp => pp.PromotionId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<PromotionProduct>()
            .HasOne(pp => pp.Product)
            .WithMany(p => p.PromotionProducts)
            .HasForeignKey(pp => pp.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        // Seed data
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Categories
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Pizza Salgada", Description = "Pizzas salgadas deliciosas", Order = 1, IsActive = true },
            new Category { Id = 2, Name = "Pizza Doce", Description = "Pizzas doces irresistíveis", Order = 2, IsActive = true },
            new Category { Id = 3, Name = "Bebidas", Description = "Bebidas geladas e refrescantes", Order = 3, IsActive = true },
            new Category { Id = 4, Name = "Sobremesas", Description = "Sobremesas deliciosas", Order = 4, IsActive = true }
        );

        // Products - Pizza Salgada
        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = 1,
                Name = "Margherita",
                Description = "Mozzarella fresca, tomate, orégano e manjericão",
                Price = 45.00m,
                ImageUrl = "/images/margherita.jpg",
                CategoryId = 1,
                Rating = 4.8m
            },
            new Product
            {
                Id = 2,
                Name = "Calabresa",
                Description = "Calabresa fatiada, cebola e mozzarella",
                Price = 48.00m,
                ImageUrl = "/images/calabresa.jpg",
                CategoryId = 1,
                Rating = 4.9m
            },
            new Product
            {
                Id = 3,
                Name = "Frango com Requeijão",
                Description = "Peito de frango desfiado, requeijão cremoso",
                Price = 52.00m,
                ImageUrl = "/images/frango-requeijao.jpg",
                CategoryId = 1,
                Rating = 4.7m
            },
            new Product
            {
                Id = 4,
                Name = "Portuguesa",
                Description = "Presunto, ovo, cebola, azeitona e mozzarella",
                Price = 55.00m,
                ImageUrl = "/images/portuguesa.jpg",
                CategoryId = 1,
                Rating = 4.6m
            }
        );

        // Products - Pizza Doce
        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = 5,
                Name = "Chocolate",
                Description = "Chocolate derretido e granulado",
                Price = 42.00m,
                ImageUrl = "/images/chocolate.jpg",
                CategoryId = 2,
                Rating = 4.9m
            },
            new Product
            {
                Id = 6,
                Name = "Banana com Canela",
                Description = "Banana fresca, canela e açúcar",
                Price = 40.00m,
                ImageUrl = "/images/banana-canela.jpg",
                CategoryId = 2,
                Rating = 4.8m
            },
            new Product
            {
                Id = 7,
                Name = "Morango com Nutella",
                Description = "Morango fresco e Nutella derretida",
                Price = 48.00m,
                ImageUrl = "/images/morango-nutella.jpg",
                CategoryId = 2,
                Rating = 5.0m
            }
        );

        // Products - Bebidas
        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = 8,
                Name = "Refrigerante 2L",
                Description = "Escolha: Coca, Guaraná ou Fanta",
                Price = 12.00m,
                ImageUrl = "/images/refrigerante.jpg",
                CategoryId = 3,
                Rating = 4.5m
            },
            new Product
            {
                Id = 9,
                Name = "Suco Natural 500ml",
                Description = "Laranja, maçã ou melancia",
                Price = 10.00m,
                ImageUrl = "/images/suco.jpg",
                CategoryId = 3,
                Rating = 4.7m
            },
            new Product
            {
                Id = 10,
                Name = "Água 1.5L",
                Description = "Água mineral gelada",
                Price = 5.00m,
                ImageUrl = "/images/agua.jpg",
                CategoryId = 3,
                Rating = 4.6m
            }
        );

        // Products - Sobremesas
        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = 11,
                Name = "Sorvete Italiano",
                Description = "Pote 500ml de sorvete italiano",
                Price = 18.00m,
                ImageUrl = "/images/sorvete.jpg",
                CategoryId = 4,
                Rating = 4.8m
            },
            new Product
            {
                Id = 12,
                Name = "Brownie",
                Description = "Brownie quente com calda de chocolate",
                Price = 15.00m,
                ImageUrl = "/images/brownie.jpg",
                CategoryId = 4,
                Rating = 4.9m
            }
        );

        // Promotions
        modelBuilder.Entity<Promotion>().HasData(
            new Promotion
            {
                Id = 1,
                Title = "Combo 2 Pizzas + Bebida",
                Description = "2 pizzas salgadas grandes + 1 Refrigerante 2L por apenas R$ 99,90",
                DiscountAmount = 15.00m,
                MinimumOrderValue = 99.90m,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(30),
                IsActive = true,
                DisplayOrder = 1
            },
            new Promotion
            {
                Id = 2,
                Title = "20% OFF em Pizzas Doces",
                Description = "20% de desconto em toda pizza doce",
                DiscountPercentage = 20,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(15),
                IsActive = true,
                DisplayOrder = 2
            }
        );
    }
}
