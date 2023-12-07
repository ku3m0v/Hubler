namespace Hubler.DAL.Models;

public class ExpiredWarehouse
{
    public int Warehouse_Id { get; set; }
    public int ProductId { get; set; }
    public string Product_Title { get; set; }
    public int Quantity { get; set; }
    public int SupermarketId { get; set; }
    public string Supermarket_Title { get; set; }
    public DateTime ExpiryDate { get; set; }
}