namespace Hubler.DAL.Models;

public class ExpiredInventory
{
    public int InventoryId { get; set; }
    public int ProductId { get; set; }
    public string ProductTitle { get; set; }
    public int Quantity { get; set; }
    public int SupermarketId { get; set; }
    public string SupermarketTitle { get; set; }
    public DateTime ExpiryDate { get; set; }
}