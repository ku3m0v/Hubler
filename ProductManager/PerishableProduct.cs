namespace Hubler.ProductManager;

public class PerishableProduct
{
    public string Title { get; set; }
    public decimal Price { get; set; }
    public DateTime ExpirationDate { get; set; }
    public string StorageType { get; set; }
}