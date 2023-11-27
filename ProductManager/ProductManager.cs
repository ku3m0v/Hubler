
namespace Hubler.ProductManager;

public class ProductManager
{
    public List<PerishableProduct> GetPerishableProducts()
    {
        var products = new List<PerishableProduct>();
        products.Add(new PerishableProduct
        {
            Title = "Milk",
            Price = 2.99m,
            ExpirationDate = DateTime.Now.AddDays(7),
            StorageType = "Refrigerated"
        });
        products.Add(new PerishableProduct
        {
            Title = "Eggs",
            Price = 1.99m,
            ExpirationDate = DateTime.Now.AddDays(14),
            StorageType = "Refrigerated"
        });
        products.Add(new PerishableProduct
        {
            Title = "Bread",
            Price = 1.99m,
            ExpirationDate = DateTime.Now.AddDays(7),
            StorageType = "Room Temperature"
        });
        products.Add(new PerishableProduct
        {
            Title = "Bananas",
            Price = 0.99m,
            ExpirationDate = DateTime.Now.AddDays(5),
            StorageType = "Room Temperature"
        });
        products.Add(new PerishableProduct
        {
            Title = "Apples",
            Price = 1.49m,
            ExpirationDate = DateTime.Now.AddDays(14),
            StorageType = "Room Temperature"
        });
        products.Add(new PerishableProduct
        {
            Title = "Oranges",
            Price = 1.49m,
            ExpirationDate = DateTime.Now.AddDays(14),
            StorageType = "Room Temperature"
        });
        products.Add(new PerishableProduct
        {
            Title = "Potatoes",
            Price = 1.99m,
            ExpirationDate = DateTime.Now.AddDays(30),
            StorageType = "Room Temperature"
        });
        products.Add(new PerishableProduct
        {
            Title = "Onions",
            Price = 1.49m,
            ExpirationDate = DateTime.Now.AddDays(30),
            StorageType = "Room Temperature"
        });
        products.Add(new PerishableProduct
        {
            Title = "Carrots",
            Price = 1.49m,
            ExpirationDate = DateTime.Now.AddDays(30),
            StorageType = "Room Temperature"
        });
        products.Add(new PerishableProduct
        {
            Title = "Celery",
            Price = 1.99m,
            ExpirationDate = DateTime.Now.AddDays(14),
            StorageType = "Refrigerated"
        });
        return products;
    }
    
    public List<NonPerishableProduct> GetNonPerishableProducts()
    {
        var products = new List<NonPerishableProduct>();
        products.Add(new NonPerishableProduct
        {
            Title = "Cereal",
            Price = 3.99m,
            ShelfLife = 365
        });
        products.Add(new NonPerishableProduct
        {
            Title = "Pasta",
            Price = 1.99m,
            ShelfLife = 365
        });
        products.Add(new NonPerishableProduct
        {
            Title = "Rice",
            Price = 1.99m,
            ShelfLife = 365
        });
        products.Add(new NonPerishableProduct
        {
            Title = "Peanut Butter",
            Price = 2.99m,
            ShelfLife = 365
        });
        products.Add(new NonPerishableProduct
        {
            Title = "Jelly",
            Price = 2.99m,
            ShelfLife = 365
        });
        products.Add(new NonPerishableProduct
        {
            Title = "Canned Soup",
            Price = 1.99m,
            ShelfLife = 365
        });
        products.Add(new NonPerishableProduct
        {
            Title = "Canned Vegetables",
            Price = 1.99m,
            ShelfLife = 365
        });
        products.Add(new NonPerishableProduct
        {
            Title = "Canned Fruit",
            Price = 1.99m,
            ShelfLife = 365
        });
        products.Add(new NonPerishableProduct
        {
            Title = "Canned Meat",
            Price = 2.99m,
            ShelfLife = 365
        });
        products.Add(new NonPerishableProduct
        {
            Title = "Canned Fish",
            Price = 2.99m,
            ShelfLife = 365
        });
        return products;
    }
}