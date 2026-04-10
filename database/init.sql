-- Pizza Delivery Database Script
-- SQL Server

-- Create Database
CREATE DATABASE PizzaDeliveryDB;
GO

USE PizzaDeliveryDB;
GO

-- Create Tables

-- Categories Table
CREATE TABLE Categories (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX),
    IconUrl NVARCHAR(500),
    [Order] INT NOT NULL,
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE()
);

-- Products Table
CREATE TABLE Products (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX),
    Price DECIMAL(10, 2) NOT NULL,
    ImageUrl NVARCHAR(500),
    Rating DECIMAL(3, 1) DEFAULT 0,
    CategoryId INT NOT NULL FOREIGN KEY REFERENCES Categories(Id),
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 DEFAULT GETUTCDATE()
);

-- Users Table
CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FullName NVARCHAR(200) NOT NULL,
    Email NVARCHAR(256) NOT NULL UNIQUE,
    PhoneNumber NVARCHAR(20),
    PasswordHash NVARCHAR(MAX) NOT NULL,
    [Document] NVARCHAR(20), -- CPF
    DefaultAddress NVARCHAR(500),
    DefaultAddressNumber NVARCHAR(10),
    DefaultAddressComplement NVARCHAR(200),
    DefaultCity NVARCHAR(100),
    DefaultZipCode NVARCHAR(10),
    [Role] NVARCHAR(50) DEFAULT 'Customer',
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 DEFAULT GETUTCDATE()
);

-- Orders Table
CREATE TABLE Orders (
    Id INT PRIMARY KEY IDENTITY(1,1),
    OrderNumber NVARCHAR(50) NOT NULL UNIQUE,
    UserId INT NOT NULL FOREIGN KEY REFERENCES Users(Id),
    TotalAmount DECIMAL(10, 2) NOT NULL,
    DeliveryFee DECIMAL(10, 2) DEFAULT 5.00,
    DiscountAmount DECIMAL(10, 2),
    [Status] NVARCHAR(50) DEFAULT 'Pending',
    IfoodOrderId NVARCHAR(100),
    DeliveryAddress NVARCHAR(500) NOT NULL,
    DeliveryNotes NVARCHAR(MAX),
    PhoneNumber NVARCHAR(20),
    PromotionId INT,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    CompletedAt DATETIME2,
    CancelledAt DATETIME2
);

-- Order Items Table
CREATE TABLE OrderItems (
    Id INT PRIMARY KEY IDENTITY(1,1),
    OrderId INT NOT NULL FOREIGN KEY REFERENCES Orders(Id) ON DELETE CASCADE,
    ProductId INT NOT NULL FOREIGN KEY REFERENCES Products(Id),
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(10, 2) NOT NULL,
    Notes NVARCHAR(MAX)
);

-- Promotions Table
CREATE TABLE Promotions (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX),
    ImageUrl NVARCHAR(500),
    DiscountPercentage DECIMAL(5, 2),
    DiscountAmount DECIMAL(10, 2),
    MinimumOrderValue DECIMAL(10, 2),
    StartDate DATETIME2 NOT NULL,
    EndDate DATETIME2 NOT NULL,
    IsActive BIT DEFAULT 1,
    DisplayOrder INT DEFAULT 0,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE()
);

-- Promotion Products Table (Many-to-Many)
CREATE TABLE PromotionProducts (
    Id INT PRIMARY KEY IDENTITY(1,1),
    PromotionId INT NOT NULL FOREIGN KEY REFERENCES Promotions(Id) ON DELETE CASCADE,
    ProductId INT NOT NULL FOREIGN KEY REFERENCES Products(Id) ON DELETE CASCADE,
    PromotionPrice DECIMAL(10, 2)
);

-- Create Indexes
CREATE INDEX IX_Products_CategoryId ON Products(CategoryId);
CREATE INDEX IX_Orders_UserId ON Orders(UserId);
CREATE INDEX IX_Orders_Status ON Orders([Status]);
CREATE INDEX IX_Orders_CreatedAt ON Orders(CreatedAt);
CREATE INDEX IX_OrderItems_OrderId ON OrderItems(OrderId);
CREATE INDEX IX_OrderItems_ProductId ON OrderItems(ProductId);
CREATE INDEX IX_Users_Email ON Users(Email);
CREATE INDEX IX_Promotions_IsActive_EndDate ON Promotions(IsActive, EndDate);

-- Create Stored Procedures

-- Get Products by Category
CREATE PROCEDURE sp_GetProductsByCategory
    @CategoryId INT
AS
BEGIN
    SELECT * FROM Products 
    WHERE CategoryId = @CategoryId AND IsActive = 1
    ORDER BY Name;
END
GO

-- Get Active Promotions
CREATE PROCEDURE sp_GetActivePromotions
AS
BEGIN
    SELECT * FROM Promotions 
    WHERE IsActive = 1 
    AND GETUTCDATE() BETWEEN StartDate AND EndDate
    ORDER BY DisplayOrder;
END
GO

-- Get Order Details
CREATE PROCEDURE sp_GetOrderDetails
    @OrderId INT
AS
BEGIN
    SELECT 
        o.*,
        u.FullName,
        u.Email,
        u.PhoneNumber
    FROM Orders o
    INNER JOIN Users u ON o.UserId = u.Id
    WHERE o.Id = @OrderId;
    
    SELECT 
        oi.*,
        p.Name,
        p.Description,
        p.ImageUrl
    FROM OrderItems oi
    INNER JOIN Products p ON oi.ProductId = p.Id
    WHERE oi.OrderId = @OrderId;
END
GO

-- Seed Initial Data

-- Insert Categories
INSERT INTO Categories (Name, Description, IconUrl, [Order]) VALUES
('Pizza Salgada', 'Pizzas salgadas deliciosas', '/icons/salty.png', 1),
('Pizza Doce', 'Pizzas doces irresistíveis', '/icons/sweet.png', 2),
('Bebidas', 'Bebidas geladas e refrescantes', '/icons/drinks.png', 3),
('Sobremesas', 'Sobremesas deliciosas', '/icons/dessert.png', 4);

GO

-- Insert Products - Salty Pizzas
INSERT INTO Products (Name, Description, Price, ImageUrl, Rating, CategoryId) VALUES
('Margherita', 'Mozzarella fresca, tomate, orégano e manjericão', 45.00, '/images/margherita.jpg', 4.8, 1),
('Calabresa', 'Calabresa fatiada, cebola e mozzarella', 48.00, '/images/calabresa.jpg', 4.9, 1),
('Frango com Requeijão', 'Peito de frango desfiado, requeijão cremoso', 52.00, '/images/frango.jpg', 4.7, 1),
('Portuguesa', 'Presunto, ovo, cebola, azeitona e mozzarella', 55.00, '/images/portuguesa.jpg', 4.6, 1),
('Pepperoni', 'Queijo mozzarella e pepperoni crocante', 50.00, '/images/pepperoni.jpg', 4.8, 1);

-- Insert Products - Sweet Pizzas
INSERT INTO Products (Name, Description, Price, ImageUrl, Rating, CategoryId) VALUES
('Chocolate', 'Chocolate derretido e granulado', 42.00, '/images/chocolate.jpg', 4.9, 2),
('Banana com Canela', 'Banana fresca, canela e açúcar', 40.00, '/images/banana-canela.jpg', 4.8, 2),
('Morango com Nutella', 'Morango fresco e Nutella derretida', 48.00, '/images/morango-nutella.jpg', 5.0, 2),
('Goiabada com Queijo', 'Doce de goiabada com queijo mozzarella', 45.00, '/images/goiabada-queijo.jpg', 4.7, 2);

-- Insert Products - Drinks
INSERT INTO Products (Name, Description, Price, ImageUrl, Rating, CategoryId) VALUES
('Refrigerante 2L', 'Escolha: Coca, Guaraná ou Fanta', 12.00, '/images/refrigerante.jpg', 4.5, 3),
('Suco Natural 500ml', 'Laranja, maçã ou melancia', 10.00, '/images/suco.jpg', 4.7, 3),
('Água 1.5L', 'Água mineral gelada', 5.00, '/images/agua.jpg', 4.6, 3),
('Chá Gelado', 'Chá gelado refrescante', 8.00, '/images/cha.jpg', 4.5, 3);

-- Insert Products - Desserts
INSERT INTO Products (Name, Description, Price, ImageUrl, Rating, CategoryId) VALUES
('Sorvete Italiano', 'Pote 500ml de sorvete italiano', 18.00, '/images/sorvete.jpg', 4.8, 4),
('Brownie', 'Brownie quente com calda de chocolate', 15.00, '/images/brownie.jpg', 4.9, 4),
('Pavê', 'Pavê de chocolate caseiro', 16.00, '/images/pave.jpg', 4.7, 4),
('Cheesecake', 'Cheesecake com calda de morango', 20.00, '/images/cheesecake.jpg', 4.8, 4);

GO

-- Insert Promotions
INSERT INTO Promotions (Title, Description, DiscountAmount, MinimumOrderValue, StartDate, EndDate, IsActive, DisplayOrder)
VALUES
('Combo 2 Pizzas + Bebida', '2 pizzas salgadas grandes + 1 Refrigerante 2L', 15.00, 99.90, GETUTCDATE(), DATEADD(DAY, 30, GETUTCDATE()), 1, 1),
('20% OFF em Pizzas Doces', '20% de desconto em toda pizza doce', NULL, NULL, GETUTCDATE(), DATEADD(DAY, 15, GETUTCDATE()), 1, 2),
('Frete Grátis', 'Frete grátis acima de R$ 50', 5.00, 50.00, GETUTCDATE(), DATEADD(DAY, 7, GETUTCDATE()), 1, 3);

GO

PRINT 'Database setup completed successfully!';
