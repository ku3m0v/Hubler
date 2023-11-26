namespace Hubler.Models;

public class PerishableProductModel
{
    public int ProductId { get; set; }
    public string Title { get; set; }
    public decimal CurrentPrice { get; set; }
    public string ProductType { get; set; }
    public DateTime ExpiryDate { get; set; }
    public string StorageType { get; set; }
}