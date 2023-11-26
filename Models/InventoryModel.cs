namespace Hubler.Models;

public class InventoryModel
{
    public int Id { get; set; }
    public string SupermarketTitle { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public string Title { get; set; }
    public decimal CurrentPrice { get; set; }
    public string ProductType { get; set; }
    public DateTime ExpiryDate { get; set; }
    public string StorageType { get; set; }
    public int ShelfLife { get; set; }
}